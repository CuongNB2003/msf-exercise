import { Component, OnInit } from '@angular/core';
import { RoleService } from '../../../services/role/role.service';
import { CommonModule } from '@angular/common';
import { RoleResponse } from '../../../services/role/role.interface';
import moment from 'moment';
import 'moment/locale/vi';

@Component({
  selector: 'app-role',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit {
  roles: RoleResponse[] = [];
  totalRecords: number = 0;
  page: number = 1;
  limit: number = 10;

  constructor(private roleService: RoleService) { }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.roles = response.data.data;
        this.totalRecords = response.data.totalRecords;
      },
      error: (err) => {
        alert(`Không lấy được dữ liệu: ${err}`);
      },
      complete: () => console.log("Lấy dữ liệu role thành công")
    });
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.loadRoles();
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative + ", " + multiple;
  }

}
