import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-thanh-ly-hop-dong',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './thanh-ly-hop-dong.component.html',
  styleUrl: './thanh-ly-hop-dong.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ThanhLyHopDongComponent {
  ngayThanhLy: Date | null = null;
}
