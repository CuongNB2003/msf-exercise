import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../../ui/footer-layout/footer-layout.component';
import { SidebarAdminComponent } from '../../../ui/sidebar-admin/sidebar-admin.component';
import { NgScrollbar } from 'ngx-scrollbar';

@Component({
  selector: 'app-layout-admin',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarAdminComponent, NgScrollbar],
  templateUrl: './layout-admin.component.html',
  styleUrl: './layout-admin.component.scss'
})
export class LayoutAdminComponent {

}
