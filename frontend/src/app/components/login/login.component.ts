import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { RecaptchaFormsModule, RecaptchaModule } from 'ng-recaptcha';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../../ui/input/input.component';
import { ButtonComponent } from '../../ui/button/button.component';

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
  maxAttempts: number = 5;
  lockoutTime: number = 30 * 1000;
  lockoutMessage: string = '';
  isLocked: boolean = false;

  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.checkLockoutStatus();
    }
  }

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      recaptcha: ['', Validators.required]
    });
  }

  loginHandler(): void {
    if (this.loginForm.valid && !this.isLocked) {
      const email = this.loginForm.get('email')?.value;
      const passWord = this.loginForm.get('password')?.value;
      const reCaptchaToken = this.loginForm.get('recaptcha')?.value;

      const attempts = this.getAttempts();
      const lockedUntil = this.getLockedUntil();

      if (lockedUntil && new Date().getTime() < lockedUntil) {
        this.updateLockoutMessage(lockedUntil);
        return;
      }



      this.isSubmitting = true;
      this.authService.login({ email, passWord, reCaptchaToken }).subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.resetAttempts();
          localStorage.setItem('user', JSON.stringify(response.data.user));
          localStorage.setItem('accessToken', JSON.stringify(response.data.token.accessToken));
          localStorage.setItem('refreshToken', JSON.stringify(response.data.token.refreshToken));
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.isSubmitting = false;
          this.incrementAttempts();
          if (attempts + 1 >= this.maxAttempts) {
            this.lockAccount();
            const lockUntil = new Date().getTime() + this.lockoutTime;
            this.updateLockoutMessage(lockUntil);
          } else {
            alert(`Đăng nhập thất bại: ${error.message}. Bạn còn ${this.maxAttempts - attempts - 1} lần thử.`);
          }
          this.captchaRef.reset();
        },
        complete: () => {
          console.log('Login request completed');
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  isInvalid(controlName: string): boolean {
    const control = this.loginForm.get(controlName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

  private getAttempts(): number {
    return parseInt(localStorage.getItem('loginAttempts') || '0', 10);
  }

  private incrementAttempts(): void {
    const attempts = this.getAttempts() + 1;
    localStorage.setItem('loginAttempts', attempts.toString());
  }

  private resetAttempts(): void {
    localStorage.removeItem('loginAttempts');
  }

  private lockAccount(): void {
    const lockUntil = new Date().getTime() + this.lockoutTime;
    localStorage.setItem('lockedUntil', lockUntil.toString());
  }

  private getLockedUntil(): number | null {
    if (typeof window !== 'undefined' && localStorage) {
      const lockedUntil = localStorage.getItem('lockedUntil');
      return lockedUntil ? parseInt(lockedUntil, 10) : null;
    }
    return null;
  }

  private checkLockoutStatus(): void {
    const lockedUntil = this.getLockedUntil();
    if (lockedUntil && new Date().getTime() < lockedUntil) {
      this.updateLockoutMessage(lockedUntil);
    }
  }

  private updateLockoutMessage(lockUntil: number): void {
    const updateMessage = () => {
      const remainingTime = lockUntil - new Date().getTime();
      if (remainingTime <= 0) {
        clearInterval(intervalId);
        this.isLocked = false;
        this.lockoutMessage = '';
        this.resetAttempts();
        localStorage.removeItem('lockedUntil');
      } else {
        const minutes = Math.floor(remainingTime / 60000);
        const seconds = Math.floor((remainingTime % 60000) / 1000);
        this.lockoutMessage = `Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau ${seconds} giây.`;
      }
    };

    this.isLocked = true;
    updateMessage(); // Initial call to set the message immediately
    const intervalId = setInterval(updateMessage, 1000); // Update every second
  }

}
