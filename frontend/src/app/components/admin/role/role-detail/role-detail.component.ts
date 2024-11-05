import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import moment from 'moment';
import { RoleService } from '@services/role/role.service';
import { RoleResponse } from '@services/role/role.interface';
import { MessageService } from 'primeng/api';
import { MaterialModule } from '../../../../modules/material/material.module';

@Component({
  selector: 'app-role-detail',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './role-detail.component.html',
  styleUrl: './role-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]

})
export class RoleDetailComponent implements OnInit {
  constructor(
    private messageService: MessageService,
    private roleService: RoleService,
    public dialogRef: MatDialogRef<RoleDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date(),
    total: 0,
    menus: [],
    permissions: []
  };


  ngOnInit(): void {
    this.getRoleById();
  }

  getRoleById(): void {
    this.roleService.getRoleById(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.role = response.data;
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
