import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-nghiem-thu-hang-hoa',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './nghiem-thu-hang-hoa.component.html',
  styleUrl: './nghiem-thu-hang-hoa.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class NghiemThuHangHoaComponent {
  listPhieuNghiemThu: any[] = [];
  listHangHoaNghiemThu: any[] = [];

  selectedHangHoaNghiemThu: any[] = [];
  selectedDate: Date | null = null;

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {
    // Dữ liệu giả
    this.listHangHoaNghiemThu = Array.from({ length: 5 }, (_, i) => ({
      id: i,
    }));
    this.listPhieuNghiemThu = Array.from({ length: 3 }, (_, i) => ({
      id: i,
    }));
  }

  handleBlur(): void {
    if (
      this.selectedDate !== null &&
      this.selectedDate !== undefined &&
      !this.isValidDate(this.selectedDate)
    ) {
      this.selectedDate = new Date();
    }
  }

  isValidDate(date: any): boolean {
    return date instanceof Date && !isNaN(date.getTime());
  }

  openDialog() {}

  xoaHangHoaNghiemThu(event: Event) {
    if (
      !this.selectedHangHoaNghiemThu ||
      this.selectedHangHoaNghiemThu.length === 0
    ) {
      this.messageService.add({
        severity: 'Err',
        summary: 'Cảnh báo',
        detail: 'Bạn cần chọn ít nhất 1 hàng hóa nghiệm thu để xóa.',
      });
      return;
    }

    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Bạn có muốn xóa các hàng hóa nghiệm thu đã chọn ?',
      header: 'Thông báo',
      acceptIcon: 'none',
      rejectIcon: 'none',
      acceptButtonStyleClass: 'p-button cancel click',
      rejectButtonStyleClass: 'p-button delete click',
      acceptLabel: 'Hủy',
      rejectLabel: 'Xóa',
      accept: () => {},
      reject: () => {
        this.selectedHangHoaNghiemThu.forEach((item) => {
          const index = this.listHangHoaNghiemThu.indexOf(item);
          if (index > -1) {
            this.listHangHoaNghiemThu.splice(index, 1);
          }
        });
        this.selectedHangHoaNghiemThu = [];
      },
    });
  }
}
