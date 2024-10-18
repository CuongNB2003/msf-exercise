import { StorePermission } from './../../../store/store.permission';
import { StoreMenu } from './../../../store/store.menu';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserLogin } from '@services/auth/auth.interface';
import { MenuRoleResponse } from '@services/menu/menu.interface';
import { PermissionRoleResponse } from '@services/permission/permission.interface';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { FooterLayoutComponent } from '@ui/footer-layout/footer-layout.component';
import { HeaderLayoutComponent } from '@ui/header-layout/header-layout.component';
import { SidebarAdminComponent } from '@ui/sidebar-admin/sidebar-admin.component';
import { NgScrollbar } from 'ngx-scrollbar';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarAdminComponent, NgScrollbar],
  templateUrl: './layout-admin.component.html',
  styleUrl: './layout-admin.component.scss'
})
export class LayoutAdminComponent implements OnInit {
  menus: MenuRoleResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date(),
    total: 0,
    menus: [],
    permissions: []
  };

  constructor(
    private roleService: RoleService,
    private messageService: MessageService,
    private storeMenu: StoreMenu,
    private storePermission: StorePermission
  ) { }

  ngOnInit(): void {
    this.getMenu();
    this.getPermission();
  }

  private getMenu() {
    this.fetchDataFromLocalStorage<UserLogin>('user', this.loadMenus.bind(this));
  }

  private getPermission() {
    this.fetchDataFromLocalStorage<UserLogin>('user', this.loadPermissions.bind(this));
  }

  private fetchDataFromLocalStorage<T>(key: string, callback: (data: T) => void) {
    if (typeof window !== 'undefined') {
      const data = localStorage.getItem(key);
      if (data) {
        callback(JSON.parse(data) as T);
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Warning', detail: `No data found for ${key}` });
      }
    } else {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Đang chạy ở server, không thể truy cập localStorage' });
    }
  }

  private loadPermissions(user: UserLogin) {
    const permissionMap = new Map<number, PermissionRoleResponse>();
    const roleObservables = user.roles.map(role => this.roleService.getRoleById(role.id));
    forkJoin(roleObservables).subscribe({
      next: (responses) => {
        responses.forEach((response) => {
          this.role = response.data;
          this.role.permissions.forEach(permission => {
            if (!permissionMap.has(permission.id)) {
              permissionMap.set(permission.id, permission);
            }
          });
        });
        this.permissions = Array.from(permissionMap.values());
        this.storePermission.setPermissions(this.permissions);
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Permissions loaded successfully' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        console.error('Error loading roles:', err);
      },
    });
  }

  private loadMenus(user: UserLogin) {
    if (!user.roles || user.roles.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'No roles found for user' });
      return;
    }
    const menuMap = new Map<number, MenuRoleResponse>();
    const roleObservables = user.roles.map(role => this.roleService.getRoleById(role.id))
    forkJoin(roleObservables).subscribe({
      next: (responses) => {
        responses.forEach((response) => {
          this.role = response.data;
          this.role.menus.forEach(menu => {
            if (!menuMap.has(menu.id)) {
              menuMap.set(menu.id, menu);
            }
          });
        });
        this.menus = Array.from(menuMap.values());
        this.storeMenu.setMenus(this.menus);
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Menu loaded successfully' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        console.error('Error loading roles:', err); // Log lỗi
      },
    });
  }
}
