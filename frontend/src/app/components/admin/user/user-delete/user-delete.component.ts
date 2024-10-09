import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleDeleteComponent } from '@components/admin/role/role-delete/role-delete.component';
import { RoleService } from '@services/role/role.service';
import { UserService } from '@services/user/user.service';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '@ui/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-user-delete',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './user-delete.component.html',
  styleUrl: './user-delete.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserDeleteComponent {
  isSubmitting: boolean = false;

  constructor(
    private messageService: MessageService,
    public dialogRef: MatDialogRef<UserDeleteComponent>,
    public userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  deleteHandle(): void {
    this.userService.deleteUser(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
