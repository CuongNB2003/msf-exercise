import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';
import { log } from 'console';

@Component({
  selector: 'app-left-side-contract',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './left-side-contract.component.html',
  styleUrl: './left-side-contract.component.scss'
})
export class LeftSideContractComponent {
  @Input() isActive: boolean = false;
  @Output() isActiveChange = new EventEmitter<boolean>(); // Thêm dòng này
  contractForm!: FormGroup;
  isFilter: boolean = false;
  isCleanForm: boolean = false;


  viLocale: any;
  countries: any[] | undefined;
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
    { name: 'Quyết toán', value: 'TAT_CA' },

  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.countries = [
      { name: 'Việt Nam', code: 'VN' },
      { name: 'Cường Đây rồi', code: 'VN' },
      { name: 'HIHI', code: 'VN' },
    ];

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
  }

  onSubmit() {
    this.isFilter = true;
    console.log("bắt đầu: ", this.isFilter);
    
    if (this.contractForm.valid) {
      const formData = this.contractForm.value;
      console.log(formData);
      this.isFilter = false
    console.log("kết thúc: ", this.isFilter);

    }
  }

  cleanForm() {
    this.isCleanForm = true
    this.contractForm.reset();
    this.isCleanForm = false
  }

  toggleClass() {
    this.isActive = !this.isActive;
    this.isActiveChange.emit(this.isActive); // Phát sự kiện khi isActive thay đổi
  }

  toUpperCase(event: any): void {
    event.target.value = event.target.value.toUpperCase();
  }

  handleEnter(event: any, controlName: string): void {
    const inputValue = event.target.value;
    if (!inputValue || !this.isValidDate(inputValue)) {
      const today = new Date();
      const formattedDate = this.formatDate(today);
      
      // Set the formatted date to the form control
      this.contractForm.patchValue({ [controlName]: today });
    }
  }

  isValidDate(dateString: string): boolean {
    const date = Date.parse(dateString);
    return !isNaN(date);
  }

  formatDate(date: Date): string {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
  }
}
