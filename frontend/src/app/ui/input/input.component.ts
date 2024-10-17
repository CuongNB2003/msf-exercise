import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent {
  @Input() formGroup!: FormGroup;
  @Input() controlName!: string;
  @Input() placeholder!: string;
  @Input() type: string = 'text';
  @Input() className?: string;
  @Input() pattern?: string;

  isPasswordVisible: boolean = false;

  isInvalid(controlName: string): boolean {
    const control = this.formGroup.get(controlName);
    return (control && control.invalid && (control.dirty || control.touched)) ?? false;
  }

  togglePasswordVisibility(): void {
    this.isPasswordVisible = !this.isPasswordVisible;
  }

  getErrorMessage(): string | null {
    const control = this.formGroup.get(this.controlName);
    if (control?.errors) {
      if (control.errors['required']) {
        return 'Vui lòng nhập thông tin';
      } else if (control.errors['email']) {
        return 'Email không hợp lệ';
      } else if (control.errors['minlength']) {
        return `Độ dài tối thiểu là ${control.errors['minlength'].requiredLength} ký tự`;
      } else if (control.errors['maxlength']) {
        return `Độ dài tối đa là ${control.errors['minlength'].requiredLength} ký tự`;
      } else if (control.errors['pattern']) {
        return `Bạn đang ghi sai định dạng. định dạng đúng là ${this.pattern}`;
      }
    }
    return null;
  }

}
