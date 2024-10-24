import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MenuRoleResponse } from '@services/menu/menu.interface';

@Injectable({
    providedIn: 'root',
})
export class StoreMenu {
    private menusSubject = new BehaviorSubject<MenuRoleResponse[]>([]);

    menus$ = this.menusSubject.asObservable();

    setMenus(menus: MenuRoleResponse[]) {
        this.menusSubject.next(menus);
        localStorage.setItem('menus', JSON.stringify(menus));
    }

    getMenus(): MenuRoleResponse[] {
        // Kiểm tra nếu `menusSubject` có dữ liệu thì trả về
        const currentMenus = this.menusSubject.getValue();
        if (currentMenus && currentMenus.length > 0) {
            return currentMenus;
        }

        // Nếu không, lấy từ `localStorage` và cập nhật lại `menusSubject`
        const storedMenus = localStorage.getItem('menus');
        if (storedMenus) {
            const parsedMenus = JSON.parse(storedMenus);
            this.menusSubject.next(parsedMenus);
            return parsedMenus;
        }

        // Nếu không có gì thì trả về mảng rỗng
        return [];
    }

    clearPermissions() {
        this.menusSubject.next([]); // Reset BehaviorSubject về mảng rỗng
        if (typeof window !== 'undefined') {
            localStorage.removeItem('menus'); // Xóa permissions khỏi localStorage
        }
    }

}

