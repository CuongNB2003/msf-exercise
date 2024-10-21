import { PermissionService } from './../../../../services/permission/permission.service';
import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PermissionResponse, PermissionRoleResponse } from '@services/permission/permission.interface';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import moment from 'moment';
import { MessageService } from 'primeng/api';
import { PermissionDetailComponent } from '../permission-detail/permission-detail.component';
import { PermissionCreateUpdateComponent } from '../permission-create-update/permission-create-update.component';
import { PermissionDeleteComponent } from '../permission-delete/permission-delete.component';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { StorePermission } from '../../../../store/store.permission';

@Component({
  selector: 'app-permission-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent, NgxSkeletonLoaderModule],
  templateUrl: './permission-list.component.html',
  styleUrl: './permission-list.component.scss'
})
export class PermissionListComponent {
  listPermission: PermissionResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};
  isLoading: boolean = true;

  constructor(
    private messageService: MessageService,
    private permissionService: PermissionService,
    private storePermission: StorePermission,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadRoles();
    this.permissions = this.storePermission.getPermissions();
  }

  loadRoles(): void {
    this.isLoading = true;
    this.permissionService.getPermissionAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.isLoading = false;
        // this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.listPermission = response.data.data;
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
    for (const perId in this.isDropdownOpen) {
      if (this.isDropdownOpen.hasOwnProperty(perId)) {
        if (!targetElement.closest('.dropdown') && !targetElement.closest('.dropdown-toggle')) {
          this.isDropdownOpen[perId] = false;
        }
      }
    }
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.page = newPage;
    this.loadRoles();
  }

  openDialogDetail(id: number): void {
    const dialogRef = this.dialog.open(PermissionDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

  openDialogCreate(): void {
    const dialogRef = this.dialog.open(PermissionCreateUpdateComponent, {
      width: '600px',
      data: {
        id: null,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogUpdate(id: number): void {
    const dialogRef = this.dialog.open(PermissionCreateUpdateComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogDelete(id: number): void {
    const dialogRef = this.dialog.open(PermissionDeleteComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }
}

