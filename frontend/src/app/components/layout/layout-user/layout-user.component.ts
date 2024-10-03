import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../../ui/footer-layout/footer-layout.component';
import { SidebarIconComponent } from '../../../ui/sidebar-icon/sidebar-icon.component';

@Component({
  selector: 'app-layout-user',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarIconComponent],
  templateUrl: './layout-user.component.html',
  styleUrl: './layout-user.component.scss'
})
export class LayoutUserComponent {

}
