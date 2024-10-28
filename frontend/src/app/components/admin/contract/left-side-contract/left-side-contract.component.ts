import { Component, EventEmitter, Input, Output } from '@angular/core';
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
  @Input() isActive: boolean = false;
  @Output() isActiveChange = new EventEmitter<boolean>();
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
    this.products = [
      { code: 'P001P001P001P001P001P001P001P001P001P001P001P001P001P001P001', name: 'Sản phẩm A', category: 'Loại 1', quantity: 100 },
      { code: 'P002', name: 'Sản phẩm B', category: 'Loại 2', quantity: 200 },
      { code: 'P003', name: 'Sản phẩm C', category: 'Loại 3', quantity: 150 },
      { code: 'P004', name: 'Sản phẩm D', category: 'Loại 1', quantity: 75 },
      { code: 'P005', name: 'Sản phẩm E', category: 'Loại 2', quantity: 120 },
      { code: 'P001', name: 'Sản phẩm A', category: 'Loại 1', quantity: 100 },
      { code: 'P002', name: 'Sản phẩm B', category: 'Loại 2', quantity: 200 },
      { code: 'P003', name: 'Sản phẩm C', category: 'Loại 3', quantity: 150 },
      { code: 'P004', name: 'Sản phẩm D', category: 'Loại 1', quantity: 75 },
      { code: 'P005', name: 'Sản phẩm E', category: 'Loại 2', quantity: 120 },
      { code: 'P001', name: 'Sản phẩm A', category: 'Loại 1', quantity: 100 },
      { code: 'P002', name: 'Sản phẩm B', category: 'Loại 2', quantity: 200 },
      { code: 'P003', name: 'Sản phẩm C', category: 'Loại 3', quantity: 150 },
      { code: 'P004', name: 'Sản phẩm D', category: 'Loại 1', quantity: 75 },
      { code: 'P005', name: 'Sản phẩm E', category: 'Loại 2', quantity: 120 }
    ];
    
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

  toggleClass(): void {
    this.isActive = !this.isActive;
    this.isActiveChange.emit(this.isActive);
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

  private formatDate(date: Date): string {
    return `${String(date.getDate()).padStart(2, '0')}/${String(date.getMonth() + 1).padStart(2, '0')}/${date.getFullYear()}`;
  }
}
