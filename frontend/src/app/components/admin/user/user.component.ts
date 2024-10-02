import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { UserResponse } from '../../../services/user/user.interface';
import { UserService } from '../../../services/user/user.service';
import moment from 'moment';
import 'moment/locale/vi';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit {
  users: UserResponse[] = [];
  totalRecords: number = 0;
  page: number = 1;
  limit: number = 10;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }


  loadUsers(): void {
    this.userService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.users = response.data.data;
        this.totalRecords = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu user thành công")
    });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.loadUsers();
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative + ", " + multiple;
  }
}
