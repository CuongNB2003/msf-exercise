import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { StoreMenu } from '../../store/store.menu';
import { StorePermission } from '../../store/store.permission';
import { PermissionRoleResponse } from '@services/permission/permission.interface';

export const menuGuard: CanActivateFn = (route, state) => {
  const storeMenu = inject(StoreMenu);
  const storePermission = inject(StorePermission); // Inject StorePermission
  const router = inject(Router);

  // Kiểm tra xem có menu nào trùng với URL không
  const routePath = route.url.map(segment => segment.path).join('/');
  const menus = storeMenu.getMenus();
  const matchedMenu = menus.find(menu => menu.url === `/admin/${routePath}`);
  if (!matchedMenu) {
    router.navigate(['/not-found']);
    return false;
  }

  // Kiểm tra quyền view dựa trên displayName
  const permissions = storePermission.getPermissions();
  const viewPermission = `${matchedMenu.displayName}.View`;
  const hasViewPermission = hasPermission(viewPermission, permissions)
  console.log("kết quả kiểm tra permission từ menuGuard: ", hasViewPermission);

  if (!hasViewPermission) {
    router.navigate(['/not-found']);
    return false;
  }

  return true;
};

function hasPermission(permissionName: string, permissions: PermissionRoleResponse[]): boolean {
  return permissions.some(permission => permission.permissionName === permissionName);
}
