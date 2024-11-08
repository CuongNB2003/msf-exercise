import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-chi-tiet-hang-hoa-nghiem-thu',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './chi-tiet-hang-hoa-nghiem-thu.component.html',
  styleUrl: './chi-tiet-hang-hoa-nghiem-thu.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ChiTietHangHoaNghiemThuComponent {
  listNghiemThuHopDong: any[] = [];

  constructor() {
    this.listNghiemThuHopDong = Array.from({ length: 3 }, (_, i) => ({
      id: i,
    }));
  }
}
