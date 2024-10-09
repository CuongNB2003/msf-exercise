import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterInput } from '@services/auth/auth.interface';
import { AuthService } from '@services/auth/auth.service';
import { ButtonComponent } from '@ui/button/button.component';
import { InputComponent } from '@ui/input/input.component';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, InputComponent, ButtonComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;
  hidePassWord = true;
  hideConfirmPass = true;
  isSubmitting = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  registerHandler(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    const { fullName, email, password } = this.getFormValues();
    this.isSubmitting = true;
    const input: RegisterInput = {
      name: fullName,
      email: email,
      passWord: password,
      avatar: ''
    };

    this.authService.register(input).subscribe({
      next: () => this.handleSuccessfulRegister(),
      error: (error) => this.handleRegisterError(error),
      complete: () => console.log('Đăng ký thành công')
    });
  }

  private getFormValues(): any {
    return {
      fullName: this.registerForm.get('fullName')?.value,
      email: this.registerForm.get('email')?.value,
      password: this.registerForm.get('password')?.value,
    };
  }

  private handleSuccessfulRegister(): void {
    this.isSubmitting = false;
    this.router.navigate(['/login']);
  }

  private handleRegisterError(error: any): void {
    this.isSubmitting = false;
    alert(`Đăng ký thất bại: ${error}`);
  }
}

