import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { PaginationComponent } from '../../../../ui/pagination/pagination.component';
import { UserService } from '../../../../services/user/user.service';
import { UserResponse } from '../../../../services/user/user.interface';
import moment from 'moment';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users: UserResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};


  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }


  loadUsers(): void {
    this.userService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.users = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu user thành công")
    });
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative + ", " + multiple;
  }

  toggleDropdown(id: number) {
    this.isDropdownOpen[id] = !this.isDropdownOpen[id];
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.page = newPage;
    this.loadUsers();
  }
}
