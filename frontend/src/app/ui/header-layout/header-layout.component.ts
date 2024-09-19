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
  constructor(private authService: AuthService) { }
  username: string = "";
  email: string = "";
  userImage: string | null = null;
  defaultImage: string = 'avatar.jpg';
  isProfileVisible: boolean = false;

  ngOnInit(): void {
    const userDataString = localStorage.getItem('userData');
    if (userDataString) {
      const userData = JSON.parse(userDataString);
      this.username = userData.fullName;
      this.userImage = userData.avatar;
      this.email = userData.email;
    }

    if (!this.userImage) {
      this.userImage = this.defaultImage;
    }
  }

  toggleProfile(): void {
    this.isProfileVisible = !this.isProfileVisible;
  }

  onLogout() {
    this.authService.logout();
  }
}
