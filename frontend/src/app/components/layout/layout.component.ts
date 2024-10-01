import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../ui/footer-layout/footer-layout.component';
import { SidebarIconComponent } from '../../ui/sidebar-icon/sidebar-icon.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarIconComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {

}
