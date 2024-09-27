import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '../../ui/header-layout/header-layout.component';
import { FooterLayoutComponent } from '../../ui/footer-layout/footer-layout.component';
import { SidebarLayoutComponent } from '../../ui/sidebar-layout/sidebar-layout.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, FooterLayoutComponent, SidebarLayoutComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {

}
