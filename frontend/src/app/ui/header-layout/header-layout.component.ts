import { StoreSidebar } from './../../store/store.sidebar';
import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import {
  matAppsOutline,
  matDensityMediumOutline,
  matLightbulbOutline,
  matNotificationsOutline,
  matSearchOutline,
  matSettingsOutline,
  matVideocamOutline,
} from '@ng-icons/material-icons/outline';
import { Router } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';
import { UserLogin } from '@services/auth/auth.interface';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-header-layout',
  standalone: true,
  imports: [NgIconComponent, CommonModule],
  templateUrl: './header-layout.component.html',
  styleUrl: './header-layout.component.scss',
  viewProviders: [
    provideIcons({
      matAppsOutline,
      matVideocamOutline,
      matDensityMediumOutline,
      matNotificationsOutline,
      matSettingsOutline,
      matLightbulbOutline,
      matSearchOutline,
    }),
  ],
})
export class HeaderLayoutComponent implements OnInit {
  username: string = "";
  role: string = "";
  email: string = "";
  userImage: string | null = null;
  defaultImage: string = 'avatar.jpg';
  isProfileVisible: boolean = false;
  isSideBar: boolean = true;

  constructor(
    private authService: AuthService,
    private router: Router,
    private messageService: MessageService,
    private storeSidebar: StoreSidebar,
  ) { }

  ngOnInit(): void {
    const userDataString = localStorage.getItem('user');
    if (userDataString) {
      const userData: UserLogin = JSON.parse(userDataString) as UserLogin;
      this.username = userData.name;
      this.role = userData.roles.some(role => role.name === "user") ? "" : userData.roles[0].name
      this.email = userData.email;
      this.userImage = userData.avatar;
    }

    if (!this.userImage || this.userImage == 'string') {
      this.userImage = this.defaultImage;
    }
  }

  toggleSidebar() {
    this.storeSidebar.toggleSidebar();
    this.isSideBar = !this.isSideBar
  }

  onLogout() {
    this.authService.logout().subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'info', summary: 'Info', detail: res.message });
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('permissions');
        localStorage.removeItem('menus');
        localStorage.removeItem('user');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: error });
      },
      complete: () => console.log('Đăng xuất thành công')
    })
  }

  toggleProfile(event: Event) {
    event.stopPropagation();
    this.isProfileVisible = !this.isProfileVisible;
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const targetElement = event.target as HTMLElement;
    if (!targetElement.closest('.profile-container') && !targetElement.closest('.main-link')) {
      this.isProfileVisible = false;
    }
  }
}
