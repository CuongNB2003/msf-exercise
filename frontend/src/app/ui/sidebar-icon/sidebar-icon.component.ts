import { Component } from '@angular/core';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { matDatasetOutline } from '@ng-icons/material-icons/outline';

@Component({
  selector: 'app-sidebar-layout',
  standalone: true,
  imports: [NgIconComponent],
  templateUrl: './sidebar-icon.component.html',
  styleUrl: './sidebar-icon.component.scss',
  viewProviders: [provideIcons({ matDatasetOutline })],
})
export class SidebarIconComponent {

}
