import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderLayoutComponent } from '@ui/header-layout/header-layout.component';
import { SidebarUserComponent } from '@ui/sidebar-user/sidebar-user.component';

@Component({
  selector: 'app-layout-user',
  standalone: true,
  imports: [RouterModule, HeaderLayoutComponent, SidebarUserComponent],
  templateUrl: './layout-user.component.html',
  styleUrl: './layout-user.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LayoutUserComponent {

}
