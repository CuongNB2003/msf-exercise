import { Component, CUSTOM_ELEMENTS_SCHEMA, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PrimeModule } from '@modules/prime/prime.module';

import { LeftSideContractComponent } from '../left-side-contract/left-side-contract.component';
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
  styleUrls: ['./layout-contract.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class LayoutContractComponent {
  @ViewChild(NoiDungComponent)
  noiDungComponent!: NoiDungComponent;

  isActive: boolean = false;
  isAdd: boolean = true;

  onSave(): void {
    if (this.noiDungComponent && !this.noiDungComponent.validateForm()) {
      console.log('form chưa hợp lệ');
      return;
    }
    const formData = this.noiDungComponent.addContractForm.getRawValue();
    const rowData = this.noiDungComponent.getListDuLieuHangHoa();
    console.log(formData);
    console.log(rowData);
  }

  onAdd(): void {
    this.isAdd = !this.isAdd;
  }

  onCancel(): void {
    this.noiDungComponent.cleanForm();
    this.isAdd = !this.isAdd;
  }

  toggleClass(): void {
    this.isActive = !this.isActive;
  }
}
