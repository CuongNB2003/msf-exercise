import { Permission } from './../../../../services/config/permission.enum';
import { MatDialog } from '@angular/material/dialog';
import { MenuService } from './../../../../services/menu/menu.service';
import { Component, HostListener } from '@angular/core';
import { MenuResponse } from '@services/menu/menu.interface';
import moment from 'moment';
import { MessageService } from 'primeng/api';
import { MenuDetailComponent } from '../menu-detail/menu-detail.component';
import { MenuCreateUpdateComponent } from '../menu-create-update/menu-create-update.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import { MenuDeleteComponent } from '../menu-delete/menu-delete.component';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { StorePermission } from '../../../../store/store.permission';
import { PermissionRoleResponse } from '@services/permission/permission.interface';
@Component({
  selector: 'app-menu-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent, NgxSkeletonLoaderModule],
  templateUrl: './menu-list.component.html',
  styleUrl: './menu-list.component.scss'
})
export class MenuListComponent {
  P = Permission;
  listMenu: MenuResponse[] = [];
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
    private menuService: MenuService,
    private storePermission: StorePermission,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadRoles();
    this.permissions = this.storePermission.getPermissions();
  }

  loadRoles(): void {
    this.isLoading = true;
    this.menuService.getMenuAll(this.page, this.limit).subscribe({
      next: (response) => {
        this.isLoading = false;
        // this.messageService.add({ severity: 'success', summary: 'Thành công', detail: response.message });
        this.listMenu = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        this.isLoading = false;
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: err });
      },
    });
  }

  hasPermission(permissionName: Permission): boolean {
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
    this.loadRoles();
  }

  openDialogDetail(id: number): void {
    const dialogRef = this.dialog.open(MenuDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

  openDialogCreate(): void {
    const dialogRef = this.dialog.open(MenuCreateUpdateComponent, {
      width: '600px',
      data: {
        id: null,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogUpdate(id: number): void {
    const dialogRef = this.dialog.open(MenuCreateUpdateComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogDelete(id: number): void {
    const dialogRef = this.dialog.open(MenuDeleteComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }
}
