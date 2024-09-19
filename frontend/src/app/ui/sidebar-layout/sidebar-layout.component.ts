import { Component } from '@angular/core';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { matDatasetOutline } from '@ng-icons/material-icons/outline';

@Component({
  selector: 'app-sidebar-layout',
  standalone: true,
  imports: [NgIconComponent],
  templateUrl: './sidebar-layout.component.html',
  styleUrl: './sidebar-layout.component.scss',
  viewProviders: [provideIcons({ matDatasetOutline })],
})
export class SidebarLayoutComponent {

}
