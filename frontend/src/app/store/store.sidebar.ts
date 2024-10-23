// sidebar.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class StoreSidebar {
    private sidebarVisibleSubject = new BehaviorSubject<boolean>(true); // Mặc định là hiển thị
    sidebarVisible$ = this.sidebarVisibleSubject.asObservable();

    toggleSidebar() {
        this.sidebarVisibleSubject.next(!this.sidebarVisibleSubject.value); // Chuyển đổi trạng thái
    }
}
