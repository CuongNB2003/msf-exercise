import { CommonModule } from '@angular/common';
import { RoleService } from './../../services/role/role.service';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { UserLogin } from '@services/auth/auth.interface';
import { MenuRoleResponse } from '@services/menu/menu.interface';
import { RoleResponse } from '@services/role/role.interface';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-sidebar-admin',
  standalone: true,
  imports: [NgScrollbarModule, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './sidebar-admin.component.html',
  styleUrl: './sidebar-admin.component.scss'
})
export class SidebarAdminComponent implements OnInit {
  menus: MenuRoleResponse[] = [];
  isOpen = false;
  role: RoleResponse = {
    id: 0,
    name: '',
    countUser: 0,
    createdAt: new Date(),
    total: 0,
    menus: [],
    permissions: []
  };

  constructor(private roleService: RoleService, private messageService: MessageService) { }

  ngOnInit(): void {
    // this.getMenu();
  }

  toggleDropdown(event: MouseEvent) {
    this.isOpen = !this.isOpen;
    const dropdownToggle = event.currentTarget as HTMLElement;
    dropdownToggle.classList.toggle('open', this.isOpen);
  }
}