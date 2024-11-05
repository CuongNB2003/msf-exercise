import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RoleService } from '@services/role/role.service';
import { MaterialModule } from '../../../../modules/material/material.module';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-role-delete',
  standalone: true,
  imports: [
    MaterialModule,
  ],
  templateUrl: './role-delete.component.html',
  styleUrl: './role-delete.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RoleDeleteComponent {
  isSubmitting: boolean = false;

  constructor(
    private messageService: MessageService,
    public dialogRef: MatDialogRef<RoleDeleteComponent>,
    public roleService: RoleService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  deleteHandle(): void {
    this.roleService.deleteRole(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.isSubmitting = false;
        this.close()
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
        this.isSubmitting = false;
      }
    });
  }

  close(): void {
    this.data.load();
    this.dialogRef.close();
  }
}
