import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';
@Component({
  selector: 'app-home-admin',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './home-admin.component.html',
  styleUrl: './home-admin.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeAdminComponent {
  isActive = false;

  toggleClass() {
    this.isActive = !this.isActive;
  }
}
