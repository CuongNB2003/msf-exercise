import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../ui/footer-layout/footer-layout.component';
import { SidebarLayoutComponent } from '../../ui/sidebar-layout/sidebar-layout.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarLayoutComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'

})
export class AdminComponent {

}
