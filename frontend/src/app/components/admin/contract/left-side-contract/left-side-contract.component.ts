import { Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-left-side-contract',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './left-side-contract.component.html',
  styleUrls: ['./left-side-contract.component.scss']
})
export class LeftSideContractComponent {
  contractForm!: FormGroup;
  isFilter = false;
  isCleanForm = false;
  products: any[] = [];
  viLocale: any;
  countries = [
    { name: 'Việt Nam', code: 'VN' },
    { name: 'Cường Đây rồi', code: 'VN' },
    { name: 'HIHI', code: 'VN' }
  ];

  listStatus = [
    { name: 'Lưu tạm', value: 'LUU_TAM' },
    { name: 'Chính thức', value: 'CHINH_THUC' },
    { name: 'Tất cả', value: 'TAT_CA' }
  ];

  listProgress = [
    { name: 'Chưa nghiệm thu', value: 'LUU_TAM' },
    { name: 'Nghiệm thu một phần', value: 'CHINH_THUC' },
    { name: 'Hoàn thành nghiệm thu', value: 'TAT_CA' },
    { name: 'Thanh lý', value: 'TAT_CA' },
    { name: 'Quyết toán', value: 'TAT_CA' }
  ];

  constructor(private fb: FormBuilder) {
    this.viLocale = {
      firstDayOfWeek: 1,
      dayNames: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
      dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
      dayNamesMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
      monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
      monthNamesShort: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
      today: 'Hôm nay',
      clear: 'Xóa'
    };
    // Dữ liệu giả
    this.products = Array.from({ length: 100 }, (_, i) => ({
      id: i,
      code: `P00${i}`,
      name: `Sản phẩm ${i}`,
      category: `Loại ${i % 3 + 1}`,
      quantity: 100 + i
    }));
    
  }

  ngOnInit(): void {
    this.contractForm = this.fb.group({
      namKinhPhi: [null],
      soHopDong: [null],
      loaiHopDong: [null],
      tenHopDong: [null],
      trangThai: [null],
      donViCungCap: [null],
      tuNgay: [null],
      denNgay: [null],
      lanhDaoPhuTrach: [null],
      canBoPhuTrach: [null],
      tenHang: [null],
      maHang: [null],
      tienDoHopDong: [null]
    });
  }

  onSubmit(): void {
    this.isFilter = true;
    console.log('Bắt đầu:', this.isFilter);

    if (this.contractForm.valid) {
      console.log(this.contractForm.value);
      this.isFilter = false;
      console.log('Kết thúc:', this.isFilter);
    }
  }

  cleanForm(): void {
    this.isCleanForm = true;
    this.contractForm.reset();
    this.isCleanForm = false;
  }

  toUpperCase(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.value = input.value.toUpperCase();
  }

  handleEnter(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement;
    if (!input.value || !this.isValidDate(input.value)) {
      const today = new Date();
      this.contractForm.patchValue({ [controlName]: today });
    }
  }

  private isValidDate(dateString: string): boolean {
    return !isNaN(Date.parse(dateString));
  }
}
