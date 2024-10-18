import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { PaginationComponent } from '@ui/pagination/pagination.component';
import moment from 'moment';
import 'moment/locale/vi';
import { RoleDetailComponent } from '../role-detail/role-detail.component';
import { RoleCreateUpdateComponent } from '../role-create-update/role-create-update.component';
import { MessageService } from 'primeng/api';
import { RoleDeleteComponent } from '../role-delete/role-delete.component';
import { StorePermission } from '../../../../store/store.permission';
import { PermissionRoleResponse } from '@services/permission/permission.interface';

@Component({
  selector: 'app-role-list',
  standalone: true,
  imports: [CommonModule, PaginationComponent],
  templateUrl: './role-list.component.html',
  styleUrl: './role-list.component.scss'
})
export class RoleListComponent {
  roles: RoleResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  totalItems: number = 0;
  page: number = 1;
  limit: number = 10;
  currentPage: number = this.page;
  itemsPerPage: number = this.limit;
  isDropdownOpen: { [key: number]: boolean } = {};


  constructor(
    private messageService: MessageService,
    private roleService: RoleService,
    private dialog: MatDialog,
    private storePermission: StorePermission,
  ) { }

  ngOnInit(): void {
    this.loadRoles();
    this.storePermission.permissions$.subscribe(permissions => {
      this.permissions = permissions;
    });
  }

  hasPermission(permissionName: string): boolean {
    return this.permissions.some(permission => permission.permissionName === permissionName);
  }

  loadRoles(): void {
    this.roleService.getRoleAll(this.page, this.limit).subscribe({
      next: (response) => {
        // this.messageService.add({ severity: 'success', summary: 'Success', detail: response.message });
        this.roles = response.data.data;
        this.totalItems = response.data.totalRecords;
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
      },
    });
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
    const dialogRef = this.dialog.open(RoleDetailComponent, {
      width: '600px',
      data: { id: id }
    });
  }

  openDialogCreate(): void {
    const dialogRef = this.dialog.open(RoleCreateUpdateComponent, {
      width: '600px',
      data: {
        id: null,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogUpdate(id: number): void {
    const dialogRef = this.dialog.open(RoleCreateUpdateComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }

  openDialogDelete(id: number): void {
    const dialogRef = this.dialog.open(RoleDeleteComponent, {
      width: '600px',
      data: {
        id: id,
        load: () => this.loadRoles(),
      }
    });
  }
}
