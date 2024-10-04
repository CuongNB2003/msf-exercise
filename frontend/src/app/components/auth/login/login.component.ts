import { ResponseObject, Token } from './../../../services/config/response';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RecaptchaFormsModule, RecaptchaModule } from 'ng-recaptcha';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../../../ui/input/input.component';
import { ButtonComponent } from '../../../ui/button/button.component';
import { AuthService } from '../../../services/auth/auth.service';
import moment from 'moment';
import 'moment/locale/vi';
import { LoginResponse } from '../../../services/auth/auth.interface';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RecaptchaModule, ReactiveFormsModule, CommonModule, InputComponent, ButtonComponent, RecaptchaFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  @ViewChild('captchaRef') captchaRef: any;
  loginForm: FormGroup;
  isSubmitting = false;
  maxAttempts = 5;
  lockoutTime = 30 * 1000; // 30 giây
  lockoutMessage = '';
  isLocked = false; // Sửa lỗi kiểu `boolean | undefined`

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      recaptcha: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.checkLockoutStatus();
  }

  loginHandler(): void {
    if (this.loginForm.invalid || this.isLocked) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const { email, password, recaptcha } = this.getFormValues();
    const attempts = this.getAttempts();

    if (this.isAccountLocked()) {
      this.updateLockoutMessage();
      return;
    }

    this.isSubmitting = true;
    this.authService.login({ email, passWord: password, reCaptchaToken: recaptcha }).subscribe({
      next: (response: ResponseObject<LoginResponse>) => this.handleSuccessfulLogin(response),
      error: (error) => this.handleLoginError(error, attempts),
      complete: () => console.log('Đăng nhập thành công')
    });
  }

  private handleSuccessfulLogin(response: ResponseObject<LoginResponse>): void {
    this.isSubmitting = false;
    this.resetAttempts();
    this.storeUserData(response.data);
    this.redirectUser(response.data.user.role.name);
  }

  private handleLoginError(error: any, attempts: number): void {
    this.isSubmitting = false;
    this.incrementAttempts();
    const remainingAttempts = this.maxAttempts - attempts - 1;

    if (remainingAttempts <= 0) {
      this.lockAccount();
      this.updateLockoutMessage();
    } else {
      alert(`Đăng nhập thất bại: ${error}. Bạn còn ${remainingAttempts} lần thử.`);
    }
    this.captchaRef.reset();
  }

  private getFormValues(): any {
    return {
      email: this.loginForm.get('email')?.value,
      password: this.loginForm.get('password')?.value,
      recaptcha: this.loginForm.get('recaptcha')?.value
    };
  }

  private storeUserData(data: LoginResponse): void {
    localStorage.setItem('user', JSON.stringify(data.user));
    localStorage.setItem('accessToken', JSON.stringify(data.token.accessToken));
    localStorage.setItem('refreshToken', JSON.stringify(data.token.refreshToken));
    console.log("thời gian còn lại của accessToken", this.formatDate(data.token.accessToken.expires));

  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').format('LTS');
    return relative;
  }

  private redirectUser(role: string): void {
    const route = role === 'admin' ? '/admin' : '/';
    this.router.navigate([route]);
  }

  isInvalid(controlName: string): boolean {
    const control = this.loginForm.get(controlName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

  // xử lý khóa tài khoản
  private getAttempts(): number {
    if (typeof window !== 'undefined' && localStorage) {
      return parseInt(localStorage.getItem('loginAttempts') || '0', 10);
    }
    return 0;
  }

  private incrementAttempts(): void {
    if (typeof window !== 'undefined' && localStorage) {
      const attempts = this.getAttempts() + 1;
      localStorage.setItem('loginAttempts', attempts.toString());
    }
  }

  private resetAttempts(): void {
    if (typeof window !== 'undefined' && localStorage) {
      localStorage.removeItem('loginAttempts');
    }
  }

  private lockAccount(): void {
    if (typeof window !== 'undefined' && localStorage) {
      const lockUntil = new Date().getTime() + this.lockoutTime;
      localStorage.setItem('lockedUntil', lockUntil.toString());
    }
  }

  private isAccountLocked(): boolean {
    if (typeof window !== 'undefined' && localStorage) {
      const lockedUntil = this.getLockedUntil();
      return lockedUntil !== null && new Date().getTime() < lockedUntil;
    }
    return false;
  }

  private getLockedUntil(): number | null {
    if (typeof window !== 'undefined' && localStorage) {
      const lockedUntil = localStorage.getItem('lockedUntil');
      return lockedUntil ? parseInt(lockedUntil, 10) : null;
    }
    return null;
  }

  private checkLockoutStatus(): void {
    if (this.isAccountLocked()) {
      this.updateLockoutMessage();
    }
  }

  private updateLockoutMessage(): void {
    const lockedUntil = this.getLockedUntil();
    if (!lockedUntil) return;

    const updateMessage = () => {
      const remainingTime = lockedUntil - new Date().getTime();
      if (remainingTime <= 0) {
        clearInterval(intervalId);
        this.unlockAccount();
      } else {
        const seconds = Math.floor((remainingTime % 60000) / 1000);
        this.lockoutMessage = `Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau ${seconds} giây.`;
      }
    };

    this.isLocked = true;
    updateMessage(); // Cập nhật lần đầu
    const intervalId = setInterval(updateMessage, 1000); // Cập nhật mỗi giây
  }

  private unlockAccount(): void {
    if (typeof window !== 'undefined' && localStorage) {
      this.isLocked = false;
      this.lockoutMessage = '';
      this.resetAttempts();
      localStorage.removeItem('lockedUntil');
    }
  }
}
