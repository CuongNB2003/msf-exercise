import { PermissionService } from './../../../../services/permission/permission.service';
import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PermissionInput, PermissionResponse } from '@services/permission/permission.interface';
import { IconComponent } from '@ui/icon/icon.component';
import { InputComponent } from '@ui/input/input.component';
import { MaterialModule } from '@ui/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-permission-create-update',
  standalone: true,
  imports: [
    MaterialModule,
    FormsModule,
    CommonModule,
    InputComponent,
    IconComponent,
    ReactiveFormsModule
  ],
  templateUrl: './permission-create-update.component.html',
  styleUrl: './permission-create-update.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PermissionCreateUpdateComponent {
  permissionForm: FormGroup;
  isSubmitting: boolean = false;

  permission: PermissionResponse = {
    id: 0,
    countRole: 0,
    createdAt: new Date(),
    description: '',
    permissionName: '',
    total: 0
  };

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PermissionCreateUpdateComponent>,
    private permissionService: PermissionService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.permissionForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    if (this.data.id) {
      this.loadDataRole(this.data.id);
    }
  }

  loadDataRole(id: number): void {
    this.permissionService.getPermissionById(id).subscribe({
      next: (response) => {
        this.permission = response.data;
        this.permissionForm.patchValue({
          name: this.permission.permissionName,
          description: this.permission.description,
        });
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu người dùng theo id thành công")
    });
  }

  onSubmit(): void {
    if (this.permissionForm.invalid) {
      this.permissionForm.markAllAsTouched();
      return;
    }

    if (this.data.id) {
      this.isSubmitting = true;
      this.updateHandle(this.data.id)
    } else {
      this.isSubmitting = true;
      this.createHandle()
    }
  }

  createHandle(): void {
    const input: PermissionInput = {
      permissionName: this.permissionForm.get('name')?.value,
      description: this.permissionForm.get('description')?.value,
    };

    this.permissionService.createPermission(input).subscribe({
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

  updateHandle(id: number): void {
    const input: PermissionInput = {
      permissionName: this.permissionForm.get('name')?.value,
      description: this.permissionForm.get('description')?.value,
    };

    this.permissionService.updatePermission(input, id).subscribe({
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
