import { Component, CUSTOM_ELEMENTS_SCHEMA, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserResponse } from '@services/user/user.interface';
import { UserService } from '@services/user/user.service';
import moment from 'moment';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserDetailComponent implements OnInit {

  constructor(
    private userService: UserService,
    public dialogRef: MatDialogRef<UserDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  user: UserResponse = {
    id: 0,
    name: '',
    email: '',
    roleId: 0,
    avatar: '',
    createdAt: new Date(),
    role: {
      id: 0,
      name: ''
    }
  }

  ngOnInit(): void {
    this.getRoleById();
  }

  getRoleById(): void {
    this.userService.getUserById(this.data.id).subscribe({
      next: (response) => {
        this.user = response.data;
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
