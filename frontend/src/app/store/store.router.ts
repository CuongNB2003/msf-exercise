import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class StoreRouter {
    private previousUrlSubject = new BehaviorSubject<string | null>(null);
    previousUrl$ = this.previousUrlSubject.asObservable();

    constructor() {
        // Lấy URL từ localStorage khi khởi tạo
        const previousUrl = typeof window !== 'undefined' ? localStorage.getItem('previousUrl') : null;
        this.previousUrlSubject.next(previousUrl); // Cập nhật giá trị của previousUrlSubject
    }

    setPreviousUrl(url: string) {
        this.previousUrlSubject.next(url);
        if (typeof window !== 'undefined') {
            localStorage.setItem('previousUrl', url); // Lưu URL vào localStorage
        }
    }

    getPreviousUrl(): string | null {
        return typeof window !== 'undefined' ? localStorage.getItem('previousUrl') : null; // Lấy URL từ localStorage
    }

    clearPreviousUrl() {
        this.previousUrlSubject.next(null);
        if (typeof window !== 'undefined') {
            localStorage.removeItem('previousUrl');
        }
    }
}
