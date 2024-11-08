import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-tien-do-thanh-toan',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './tien-do-thanh-toan.component.html',
  styleUrl: './tien-do-thanh-toan.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class TienDoThanhToanComponent {
  listTienDoThanhToan: any[] = [];

  constructor() {
    // Dữ liệu giả
    // this.listTienDoThanhToan = Array.from({ length: 100 }, (_, i) => ({
    //   id: i,
    // }));
  }
}
