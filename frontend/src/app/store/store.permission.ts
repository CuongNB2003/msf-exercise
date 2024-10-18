import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PermissionRoleResponse } from '@services/permission/permission.interface';

@Injectable({
    providedIn: 'root' // Service này sẽ được dùng ở mọi nơi mà không cần import
})
export class StorePermission {
    private permissionsSubject = new BehaviorSubject<PermissionRoleResponse[]>([]);

    // Trả về Observable để lắng nghe thay đổi nếu cần
    get permissions$() {
        return this.permissionsSubject.asObservable();
    }

    setPermissions(permissions: PermissionRoleResponse[]) {
        this.permissionsSubject.next(permissions);
        localStorage.setItem('permissions', JSON.stringify(permissions));
    }

    getPermissions(): PermissionRoleResponse[] {
        // Kiểm tra nếu `menusSubject` có dữ liệu thì trả về
        const currentPermissions = this.permissionsSubject.getValue();
        if (currentPermissions && currentPermissions.length > 0) {
            return currentPermissions;
        }

        // Nếu không, lấy từ `localStorage` và cập nhật lại `menusSubject`
        const storedPermissions = localStorage.getItem('menus');
        if (storedPermissions) {
            const parsedPermissions = JSON.parse(storedPermissions);
            this.permissionsSubject.next(parsedPermissions);
            return parsedPermissions;
        }

        // Nếu không có gì thì trả về mảng rỗng
        return [];
    }
}
