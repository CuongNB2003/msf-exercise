import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, ValidatorFn } from '@angular/forms';
import { InputComponent } from '../../ui/input/input.component';
import { ButtonComponent } from '../../ui/button/button.component';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { User } from '../../services/auth/auth.interface';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, InputComponent, ButtonComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  hidePassWord = true;
  hideConfirmPass = true;
  isSubmitting = false;

  ngOnInit(): void {

  }

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit(): void {
    this.registerForm.updateValueAndValidity();
    if (this.registerForm.valid) {
      this.isSubmitting = true;
      const fullName = this.registerForm.get('fullName')?.value;
      const email = this.registerForm.get('email')?.value;
      const password = this.registerForm.get('password')?.value;

      const user: User = {
        id: 1,
        fullName: fullName,
        email: email,
        password: password,
        avatar: '',
        role: '',
      }

      this.authService.register(user).subscribe({
        next: () => {
          this.isSubmitting = false;
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.isSubmitting = false;
          alert(`Đăng nhập thất bại: ${error.message}`);
        },
        complete: () => {
          console.log('Register request completed');
        }
      });

    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  isInvalid(controlName: string): boolean {
    const control = this.registerForm.get(controlName);
    return (control && control.invalid && (control.dirty || control.touched)) ?? false;
  }

}
