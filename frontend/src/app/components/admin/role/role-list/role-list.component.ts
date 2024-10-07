import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import moment from 'moment';
import { PaginationComponent } from '../../../../ui/pagination/pagination.component';
import { RoleResponse } from '../../../../services/role/role.interface';
import { RoleService } from '../../../../services/role/role.service';
import { MatDialog } from '@angular/material/dialog';
import { RoleDetailComponent } from '../role-detail/role-detail.component';

@Component({
  selector: 'app-role-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss'
})
export class RoleListComponent {
  roles: RoleResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};


  constructor(private roleService: RoleService, private dialog: MatDialog) { }

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

  openDialogDetail(id: number): void {
    const dialogRef = this.dialog.open(RoleDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

}
