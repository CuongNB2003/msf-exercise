import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-noi-dung',
  standalone: true,
  imports: [PrimeModule, CommonModule, ReactiveFormsModule],
  templateUrl: './noi-dung.component.html',
  styleUrl: './noi-dung.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class NoiDungComponent implements OnInit {
  visible: boolean = false;
  addContractForm!: FormGroup;

  listHangHoa: any[] = [];
  listHangHoaHienThi: any[] = [];
  listHangHoaDaChon: any[] = [];
  listChiTietHangHoa: any[] = [];

  selectedHangHoa: any[] = [];
  selectedHangHoaDaChon: any[] = [];
  selectedChiTietHangHoa: any[] = [];

  first: number = 0;
  rows: number = 10;

  countries = [
    { name: 'Việt Nam', code: 'VN' },
    { name: 'Cường Đây rồi', code: 'VN' },
    { name: 'HIHI', code: 'VN' },
  ];

  listStatus = [
    { name: 'Lưu tạm', value: 'LUU_TAM' },
    { name: 'Chính thức', value: 'CHINH_THUC' },
  ];

  listProgress = [
    { name: 'Chưa nghiệm thu', value: 'LUU_TAM' },
    { name: 'Nghiệm thu một phần', value: 'CHINH_THUC' },
    { name: 'Hoàn thành nghiệm thu', value: 'TAT_CA' },
    { name: 'Thanh lý', value: 'TAT_CA' },
    { name: 'Quyết toán', value: 'TAT_CA' },
  ];

  constructor(
    private fb: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {
    this.initializeForm();
    this.loadMockData();
  }

  ngOnInit(): void {
    this.updateDisplayedProducts();
  }

  validateForm(): void {
    if (this.addContractForm.invalid) {
      this.addContractForm.markAllAsTouched();
    }
  }

  cleanForm() {
    this.addContractForm.reset();
  }

  updateDisplayedProducts() {
    const start = this.first;
    const end = this.first + this.rows;
    this.listHangHoaHienThi = this.listHangHoa.slice(start, end);
  }

  onPageChange(event: any) {
    this.first = event.first;
    this.rows = event.rows;
    this.updateDisplayedProducts();
  }

  themHangHoaDaChon() {
    if (!this.selectedHangHoa || this.selectedHangHoa.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Cảnh báo',
        detail: 'Bạn chưa chọn hàng hóa nào.',
      });
      return;
    }

    this.selectedHangHoa.forEach((item) => {
      if (!this.listHangHoaDaChon.includes(item)) {
        this.listHangHoaDaChon.push(item);
      }
    });
    this.selectedHangHoa = [];
  }

  themChiTietHangHoa() {
    if (!this.listHangHoaDaChon || this.listHangHoaDaChon.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Cảnh báo',
        detail: 'Bạn chưa chọn hàng hóa nào.',
      });
      return;
    }

    this.listHangHoaDaChon.forEach((item) => {
      if (!this.listChiTietHangHoa.includes(item)) {
        this.listChiTietHangHoa.push(item);
      }
    });
    this.visible = false;
  }

  xoaHangHoaDaChon() {
    if (
      !this.selectedHangHoaDaChon ||
      this.selectedHangHoaDaChon.length === 0
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Cảnh báo',
        detail: 'Bạn chưa chọn hàng hóa nào.',
      });
      return;
    }

    this.selectedHangHoaDaChon.forEach((item) => {
      const index = this.listHangHoaDaChon.indexOf(item);
      if (index > -1) {
        this.listHangHoaDaChon.splice(index, 1);
      }
    });
    this.selectedHangHoaDaChon = [];
  }

  xoaChiTietHangHoa(event: Event) {
    if (
      !this.selectedChiTietHangHoa ||
      this.selectedChiTietHangHoa.length === 0
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Cảnh báo',
        detail: 'Bạn cần chọn ít nhất 1 hàng hóa để xóa.',
      });
      return;
    }

    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Bạn có muốn xóa các sản phẩm đã chọn ?',
      header: 'Thông báo',
      acceptIcon: 'none',
      rejectIcon: 'none',
      acceptButtonStyleClass: 'p-button cancel click',
      rejectButtonStyleClass: 'p-button delete click',
      acceptLabel: 'Hủy',
      rejectLabel: 'Xóa',
      accept: () => {},
      reject: () => {
        this.selectedChiTietHangHoa.forEach((item) => {
          const index = this.listChiTietHangHoa.indexOf(item);
          if (index > -1) {
            this.listChiTietHangHoa.splice(index, 1);
          }
        });
        this.selectedChiTietHangHoa = [];
      },
    });
  }

  showDialog() {
    this.visible = true;
  }

  closeDialog() {
    this.listHangHoaDaChon = [];
    this.visible = false;
  }

  handleEnter(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement;
    if (!input.value || !this.isValidDate(input.value)) {
      const today = new Date();
      this.addContractForm.patchValue({ [controlName]: today });
    }
  }

  private isValidDate(dateString: string): boolean {
    return !isNaN(Date.parse(dateString));
  }

  private initializeForm(): void {
    this.addContractForm = this.fb.group(
      {
        lanhDaoPhuTrach: [null],
        nguonKinhPhi: [null],

        loaiHopDong: [null, Validators.required],
        namKinhPhi: [null, [Validators.required, Validators.maxLength(4)]],
        canBoPhuTrach: [null, Validators.required],
        trangThai: [null, Validators.required],
        soHopDong: [null, [Validators.required, Validators.maxLength(50)]],
        tenHopDong: [null, [Validators.required, Validators.maxLength(250)]],
        donViCungCap: [null, Validators.required],
        ngayKy: [null, [Validators.required]],
        ngayCoHieuLuc: [null, [Validators.required]],
        soNgayThucHien: [0, [Validators.required, Validators.min(1)]],

        tienDoHopDong: [{ value: '', disabled: true }],
        ngayKetThucHopDong: [{ value: null, disabled: true }],
        giaTriHopDongBanDau: [{ value: null, disabled: true }],
        giaTriThucTe: [{ value: null, disabled: true }],
        daThanhToan: [{ value: null, disabled: true }],
        conThanhToan: [{ value: null, disabled: true }],
        ghiChu: [null, Validators.maxLength(500)],
      },
      { validators: this.dateValidator }
    );
  }

  private loadMockData(): void {
    this.listHangHoa = Array.from({ length: 100 }, (_, i) => ({
      id: i,
      code: `P00${i}`,
      name: `Sản phẩm ${i}`,
      category: `Loại ${i}`,
      quantity: 100 + i,
    }));
  }

  dateValidator(group: FormGroup): ValidationErrors | null {
    const ngayKy = group.get('ngayKy')?.value;
    const ngayCoHieuLuc = group.get('ngayCoHieuLuc')?.value;

    // Kiểm tra nếu ngày ký lớn hơn hoặc bằng ngày có hiệu lực
    if (ngayKy && ngayCoHieuLuc && new Date(ngayKy) > new Date(ngayCoHieuLuc)) {
      return { dateError: 'Phải lớn hơn hoặc bằng ngày ký' };
    }
    return null; // Không có lỗi
  }
}
