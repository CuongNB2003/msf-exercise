import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup } from '@angular/forms';

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as FormGroup;
    const password = formGroup.get('password');
    const confirmPassword = formGroup.get('confirmPass');

    if (password && confirmPassword) {
        if (password.value !== confirmPassword.value) {
            confirmPassword.setErrors({ mismatch: true });
            return { mismatch: true };
        } else {
            confirmPassword.setErrors(null);
            return null;
        }
    }
    return null;
};

