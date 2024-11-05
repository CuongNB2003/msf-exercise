import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserResponse } from '@services/user/user.interface';
import { UserService } from '@services/user/user.service';
import { MaterialModule } from '../../../../modules/material/material.module';
import moment from 'moment';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserDetailComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private userService: UserService,
    public dialogRef: MatDialogRef<UserDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  user: UserResponse = {
    id: 0,
    name: '',
    email: '',
    avatar: '',
    createdAt: new Date(),
    roles: [
      {
        id: 0,
        name: ''
      }
    ]
  };

  ngOnInit(): void {
    this.getRoleById();
  }

  getRoleById(): void {
    this.userService.getUserById(this.data.id).subscribe({
      next: (response) => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.user = response.data;
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
