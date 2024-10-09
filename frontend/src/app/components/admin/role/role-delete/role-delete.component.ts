import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { RoleService } from '@services/role/role.service';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '@ui/material/material.module';

@Component({
  selector: 'app-role-delete',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './role-delete.component.html',
  styleUrl: './role-delete.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RoleDeleteComponent {
  deleteUserFrom: FormGroup;


  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<RoleDeleteComponent>,
    public roleService: RoleService
  ) {
    this.deleteUserFrom = this.fb.group({
      name: ['', [Validators.required]]
    });
  }


  deleteHandle(): void {

  }
}
