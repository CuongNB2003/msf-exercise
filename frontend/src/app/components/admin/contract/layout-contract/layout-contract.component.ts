import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { LeftSideContractComponent } from '../left-side-contract/left-side-contract.component';
import { CommonModule } from '@angular/common';
import { PrimeModule } from '@modules/prime/prime.module';
import { PhuLucComponent } from '../tab-contract/phu-luc/phu-luc.component';
import { NoiDungComponent } from '../tab-contract/noi-dung/noi-dung.component';
import { NghiemThuHangHoaComponent } from '../tab-contract/nghiem-thu-hang-hoa/nghiem-thu-hang-hoa.component';
import { ChiTietHangHoaNghiemThuComponent } from '../tab-contract/chi-tiet-hang-hoa-nghiem-thu/chi-tiet-hang-hoa-nghiem-thu.component';
import { ThanhLyHopDongComponent } from '../tab-contract/thanh-ly-hop-dong/thanh-ly-hop-dong.component';
import { HopDongGocComponent } from '../tab-contract/hop-dong-goc/hop-dong-goc.component';
import { TienDoThanhToanComponent } from '../tab-contract/tien-do-thanh-toan/tien-do-thanh-toan.component';

@Component({
  selector: 'app-layout-contract',
  standalone: true,
  imports: [
    LeftSideContractComponent,
    CommonModule,
    PrimeModule,

    PhuLucComponent,
    NoiDungComponent,
    NghiemThuHangHoaComponent,
    ChiTietHangHoaNghiemThuComponent,
    ThanhLyHopDongComponent,
    HopDongGocComponent,
    TienDoThanhToanComponent,
  ],
  templateUrl: './layout-contract.component.html',
  styleUrl: './layout-contract.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class LayoutContractComponent {
  isActive: boolean = false;
  isAdd: boolean = true;

  toggleClass(): void {
    this.isActive = !this.isActive;
  }

  onSubmit(): void {
    this.isAdd = !this.isAdd;
  }
}
