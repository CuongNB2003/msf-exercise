import { Component, OnInit } from '@angular/core';
import { RoleService } from '../../../services/role/role.service';
import { CommonModule } from '@angular/common';
import { RoleResponse } from '../../../services/role/role.interface';
import moment from 'moment';
import 'moment/locale/vi';
import { PaginationComponent } from '../../../ui/pagination/pagination.component';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit {
  roles: RoleResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};


  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.roles = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu role thành công")
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
    this.loadRoles();
  }

}
