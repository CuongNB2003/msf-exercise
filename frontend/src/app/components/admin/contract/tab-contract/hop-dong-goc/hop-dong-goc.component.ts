import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-hop-dong-goc',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './hop-dong-goc.component.html',
  styleUrl: './hop-dong-goc.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class HopDongGocComponent {}
