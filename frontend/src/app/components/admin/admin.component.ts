import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../ui/footer-layout/footer-layout.component';
import { SidebarAdminComponent } from '../../ui/sidebar-admin/sidebar-admin.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarAdminComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'

})
export class AdminComponent {

}
