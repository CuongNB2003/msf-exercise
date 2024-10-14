import { Component } from '@angular/core';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { matDatasetOutline } from '@ng-icons/material-icons/outline';

@Component({
  selector: 'app-sidebar-user',
  standalone: true,
  imports: [NgIconComponent],
  templateUrl: './sidebar-user.component.html',
  styleUrl: './sidebar-user.component.scss',
  viewProviders: [provideIcons({ matDatasetOutline })],
})
export class SidebarUserComponent {

}
