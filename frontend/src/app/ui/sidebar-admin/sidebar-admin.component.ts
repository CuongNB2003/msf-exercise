import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NgScrollbarModule } from 'ngx-scrollbar';

@Component({
  selector: 'app-sidebar-admin',
  standalone: true,
  imports: [NgScrollbarModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar-admin.component.html',
  styleUrl: './sidebar-admin.component.scss'
})
export class SidebarAdminComponent {
  isOpen = false;

  toggleDropdown(event: MouseEvent) {
    this.isOpen = !this.isOpen;
    const dropdownToggle = event.currentTarget as HTMLElement;
    dropdownToggle.classList.toggle('open', this.isOpen);

  }
}
