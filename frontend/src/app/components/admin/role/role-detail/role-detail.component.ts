import { Component, Inject, OnInit } from '@angular/core';
import { RoleDto, RoleResponse } from '../../../../services/role/role.interface';
import { CommonModule } from '@angular/common';
import { RoleService } from '../../../../services/role/role.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import moment from 'moment';

@Component({
  selector: 'app-role-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './role-detail.component.html',
  styleUrl: './role-detail.component.scss'
})
export class RoleDetailComponent implements OnInit {
  constructor(
    private roleService: RoleService,
    public dialogRef: MatDialogRef<RoleDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date()
  }

  ngOnInit(): void {
    this.getRoleById();
  }

  getRoleById(): void {
    this.roleService.getRoleById(this.data.id).subscribe({
      next: (response) => {
        this.role = response.data;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu role theo id thành công")
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
