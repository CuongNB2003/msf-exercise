import { StorePermission } from './../../store/store.permission';
import { StoreMenu } from './../../store/store.menu';
import { CommonModule } from '@angular/common';
import { RoleService } from './../../services/role/role.service';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { UserLogin } from '@services/auth/auth.interface';
import { MenuRoleResponse } from '@services/menu/menu.interface';
import { RoleResponse } from '@services/role/role.interface';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PermissionRoleResponse } from '@services/permission/permission.interface';

@Component({
  selector: 'app-sidebar-admin',
  standalone: true,
  imports: [NgScrollbarModule, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './sidebar-admin.component.html',
  styleUrl: './sidebar-admin.component.scss'
})
export class SidebarAdminComponent implements OnInit {
  menus: MenuRoleResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  isOpen = false;

  constructor(
    private storeMenu: StoreMenu,
    private storePermission: StorePermission
  ) { }

  ngOnInit(): void {
    this.storeMenu.menus$.subscribe(menus => {
      this.storePermission.permissions$.subscribe(permissions => {
        this.permissions = permissions;
        // Lọc menu trước khi gán
        this.menus = menus
          .filter(menu => this.canViewMenu(menu)) // Filter based on permissions
          .sort(this.sortMenus);  // Apply sorting
      });
    });
  }

  private sortMenus(a: MenuRoleResponse, b: MenuRoleResponse): number {
    // Định nghĩa thứ tự ưu tiên cho các displayName
    const orderMap: { [key: string]: number } = {
      'User': 1,
      'Role': 2,
      'Menu': 3,
      'Permission': 4,
      'Log': 5
    };

    const aOrder = orderMap[a.displayName] || 999; // Nếu không có trong map, đặt giá trị lớn
    const bOrder = orderMap[b.displayName] || 999;

    // Sắp xếp theo thứ tự định sẵn
    return aOrder - bOrder;
  }

  canViewMenu(menu: MenuRoleResponse): boolean {
    return this.permissions.some(permission => {
      // Tách phần trước dấu '.' trong permissionName
      const permissionEntity = permission.permissionName.split('.')[0];
      const isViewPermission = permission.permissionName.endsWith('.View');
      return permissionEntity === menu.displayName && isViewPermission;
    });
  }


  toggleDropdown(event: MouseEvent) {
    this.isOpen = !this.isOpen;
    const dropdownToggle = event.currentTarget as HTMLElement;
    dropdownToggle.classList.toggle('open', this.isOpen);
  }

}