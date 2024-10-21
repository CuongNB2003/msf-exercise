import { StorePermission } from './../../../../store/store.permission';
import { StoreMenu } from './../../../../store/store.menu';
import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import moment from 'moment';
import 'moment/locale/vi';
import { UserDetailComponent } from '../user-detail/user-detail.component';
import { MatDialog } from '@angular/material/dialog';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import { UserResponse } from '@services/user/user.interface';
import { UserService } from '@services/user/user.service';
import { UserCreateUpdateComponent } from '../user-create-update/user-create-update.component';
import { MessageService } from 'primeng/api';
import { UserDeleteComponent } from '../user-delete/user-delete.component';
import { PermissionRoleResponse } from '@services/permission/permission.interface';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';


@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent, NgxSkeletonLoaderModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  listUser: UserResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};
  isLoading: boolean = true;

  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private messageService: MessageService,
    private storePermission: StorePermission,
  ) { }

  ngOnInit(): void {
    this.loadUsers();
    this.permissions = this.storePermission.getPermissions();
  }


  loadUsers(): void {
    this.isLoading = true;
    this.userService.getAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.isLoading = false;
        // this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.listUser = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        this.isLoading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
  }


  hasPermission(permissionName: string): boolean {
    return this.permissions.some(permission => permission.permissionName === permissionName);
  }

  formatDate(date: Date): string {
    var relative = moment(date).locale('vi').fromNow();
    var multiple = moment(date).locale('vi').format('Do MMM YYYY');
    return relative + ", " + multiple;
  }

  toggleDropdown(event: Event, roleId: number) {
    event.stopPropagation();
    if (this.isDropdownOpen[roleId]) {
      this.isDropdownOpen[roleId] = false;
    } else {
      for (const id in this.isDropdownOpen) {
        this.isDropdownOpen[id] = false;
      }
      this.isDropdownOpen[roleId] = true;
    }
  }


  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const targetElement = event.target as HTMLElement;
    for (const roleId in this.isDropdownOpen) {
      if (this.isDropdownOpen.hasOwnProperty(roleId)) {
        if (!targetElement.closest('.dropdown') && !targetElement.closest('.dropdown-toggle')) {
          this.isDropdownOpen[roleId] = false;
        }
      }
    }
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.page = newPage;
    this.loadUsers();
  }

  openDialogDetail(id: number): void {
    const dialogRef = this.dialog.open(UserDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

  openDialogCreate(): void {
    const dialogRef = this.dialog.open(UserCreateUpdateComponent, {
      width: '600px',
      data: {
        id: null,
        load: () => this.loadUsers(),
      }
    });
  }

  openDialogUpdate(id: number): void {
    const dialogRef = this.dialog.open(UserCreateUpdateComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadUsers(),
      }
    });
  }

  openDialogDelete(id: number): void {
    const dialogRef = this.dialog.open(UserDeleteComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadUsers(),
      }
    });
  }
}
