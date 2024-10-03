import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
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
import { UserLogin } from '../../services/auth/auth.interface';
import { AuthService } from '../../services/auth/auth.service';

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
  constructor(private authService: AuthService, private router: Router) { }
  username: string = "";
  role: string = "";
  email: string = "";
  userImage: string | null = null;
  defaultImage: string = 'avatar.jpg';
  isProfileVisible: boolean = false;

  ngOnInit(): void {
    const userDataString = localStorage.getItem('user');
    if (userDataString) {
      const userData: UserLogin = JSON.parse(userDataString) as UserLogin;
      this.username = userData.name;
      this.role = userData.role.name == "admin" ? "Admin" : "";
      this.email = userData.email;
      this.userImage = userData.avatar;
    }

    if (!this.userImage || this.userImage == 'string') {
      this.userImage = this.defaultImage;
    }
  }

  toggleProfile(): void {
    this.isProfileVisible = !this.isProfileVisible;
  }

  onLogout() {
    this.authService.logout().subscribe({
      next: () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        this.router.navigate(['/login']);
      },
      error(error) {
        alert(`Đăng xuất thất bại: ${error}`);
      },
      complete: () => console.log('Đăng xuất thành công')
    })
  }
}
