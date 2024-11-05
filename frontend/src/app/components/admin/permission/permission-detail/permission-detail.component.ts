import { PermissionService } from './../../../../services/permission/permission.service';
import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PermissionResponse } from '@services/permission/permission.interface';
import { MaterialModule } from '../../../../modules/material/material.module';
import moment from 'moment';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-permission-detail',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './permission-detail.component.html',
  styleUrl: './permission-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class PermissionDetailComponent {
  constructor(
    private messageService: MessageService,
    private permissionService: PermissionService,
    public dialogRef: MatDialogRef<PermissionDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  permission: PermissionResponse = {
    id: 0,
    countRole: 0,
    createdAt: new Date(),
    description: '',
    permissionName: '',
    groupName: '',
    name: '',
    total: 0
  };


  ngOnInit(): void {
    this.getPermissionById();
  }

  getPermissionById(): void {
    this.permissionService.getPermissionById(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.permission = response.data;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  formatDate(date: Date): string {
    var time = moment(date).format('D/M/YYYY h:mm A');
    return time;
  }

  close(): void {
    this.dialogRef.close();
  }
}