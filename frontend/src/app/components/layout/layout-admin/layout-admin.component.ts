import { StoreSidebar } from './../../../store/store.sidebar';
import { ChangeDetectorRef, Component, OnInit, AfterViewInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StorePermission } from './../../../store/store.permission';
import { StoreMenu } from './../../../store/store.menu';
import { UserLogin } from '@services/auth/auth.interface';
import { MenuRoleResponse } from '@services/menu/menu.interface';
import { PermissionRoleResponse } from '@services/permission/permission.interface';
import { RoleResponse } from '@services/role/role.interface';
import { RoleService } from '@services/role/role.service';
import { HeaderLayoutComponent } from '@ui/header-layout/header-layout.component';
import { SidebarAdminComponent } from '@ui/sidebar-admin/sidebar-admin.component';
import { NgScrollbar } from 'ngx-scrollbar';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { animate, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, SidebarAdminComponent, NgScrollbar, CommonModule],
  templateUrl: './layout-admin.component.html',
  styleUrl: './layout-admin.component.scss',
  animations: [
    trigger('routeAnimation', [
      transition('* => *', [
        style({ transform: 'translateX(100%)', opacity: 0 }),
        animate('1s ease-in-out', style({ transform: 'translateX(0)', opacity: 1 }))
      ])
    ])
  ]
})
export class LayoutAdminComponent implements OnInit, AfterViewInit {
  menus: MenuRoleResponse[] = [];
  permissions: PermissionRoleResponse[] = [];
  isSidebarVisible: boolean = true;
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
    private storePermission: StorePermission,
    private cd: ChangeDetectorRef,
    private storeSidebar: StoreSidebar
  ) { }

  ngOnInit(): void {
    this.getDataFromLocalStorage('user', this.loadRolesData.bind(this));
    this.storeSidebar.sidebarVisible$.subscribe(isVisible => {
      this.isSidebarVisible = isVisible;
    });
  }

  ngAfterViewInit(): void {
    // Detect changes after view initialization
    this.cd.detectChanges();
  }

  private getDataFromLocalStorage<T>(key: string, callback: (data: T) => void) {
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

  private loadRolesData(user: UserLogin) {
    if (!user.roles || user.roles.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'No roles found for user' });
      return;
    }

    const roleObservables = user.roles.map(role => this.roleService.getRoleById(role.id));
    forkJoin(roleObservables).subscribe({
      next: (responses) => {
        const menuMap = new Map<number, MenuRoleResponse>();
        const permissionMap = new Map<number, PermissionRoleResponse>();

        responses.forEach(response => {
          this.role = response.data;

          // Xử lý menu
          this.role.menus.forEach(menu => {
            if (!menuMap.has(menu.id)) {
              menuMap.set(menu.id, menu);
            }
          });

          // Xử lý permission
          this.role.permissions.forEach(permission => {
            if (!permissionMap.has(permission.id)) {
              permissionMap.set(permission.id, permission);
            }
          });
        });

        // Cập nhật menus và permissions
        this.menus = Array.from(menuMap.values());
        this.permissions = Array.from(permissionMap.values());

        this.storeMenu.setMenus(this.menus);
        this.storePermission.setPermissions(this.permissions);

        // Sử dụng detectChanges để tránh lỗi ExpressionChangedAfterItHasBeenCheckedError
        this.cd.detectChanges();

        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Menus and permissions loaded successfully' });
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: err });
        console.error('Error loading roles:', err);
      }
    });
  }
}
