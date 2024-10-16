import { PermissionService } from './../../../../services/permission/permission.service';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '@ui/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-permission-delete',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './permission-delete.component.html',
  styleUrl: './permission-delete.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PermissionDeleteComponent {
  isSubmitting: boolean = false;

  constructor(
    private messageService: MessageService,
    public dialogRef: MatDialogRef<PermissionDeleteComponent>,
    public permissionService: PermissionService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  deleteHandle(): void {
    this.permissionService.deletePermission(this.data.id).subscribe({
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

