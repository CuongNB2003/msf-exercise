import { CommonModule, DecimalPipe } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  OnInit,
} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';
import { log } from 'node:console';
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
  isDialog: boolean = false;
  addContractForm!: FormGroup;
  rowControls: any[] = [];

  listHangHoa: any[] = [];
  listHangHoaHienThi: any[] = [];
  listHangHoaDaChon: any[] = [];
  listChiTietHangHoa: any[] = [];

  selectedHangHoa: any[] = [];
  selectedHangHoaDaChon: any[] = [];
  selectedChiTietHangHoa: any[] = [];

  firstDialog: number = 0;
  rowsDialog: number = 10;

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
    private confirmationService: ConfirmationService,
    private cdr: ChangeDetectorRef
  ) {
    this.initFormHopDong();
    this.loadMockData();
  }

  ngOnInit(): void {
    this.updateDisplayedProducts();
    this.addContractForm
      .get('ngayCoHieuLuc')
      ?.valueChanges.subscribe((ngayCoHieuLuc: Date) => {
        if (ngayCoHieuLuc) {
          this.updateNgayKetThucHopDong();
        }
      });

    this.addContractForm
      .get('soNgayThucHien')
      ?.valueChanges.subscribe((value) => {
        this.updateNgayKetThucHopDong();
      });
  }

  validateForm(): boolean {
    if (this.addContractForm.invalid) {
      this.addContractForm.markAllAsTouched();
      return false;
    }
    if (this.validateAllControls()) {
      console.log(this.rowControls);
      return false;
    }
    return true;
  }

  cleanForm() {
    this.addContractForm.reset();
    this.listChiTietHangHoa = [];
  }

  updateDisplayedProducts() {
    const start = this.firstDialog;
    const end = this.firstDialog + this.rowsDialog;
    this.listHangHoaHienThi = this.listHangHoa.slice(start, end);
  }

  onPageChange(event: any) {
    this.firstDialog = event.first;
    this.rowsDialog = event.rows;
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
    this.initFormHangHoa();
    this.closeDialog();
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
    this.isDialog = true;
  }

  closeDialog() {
    this.listHangHoaDaChon = [];
    this.isDialog = false;
  }

  handleEnter(event: Event, controlName: string): void {
    const input = event.target as HTMLInputElement;
    if (!input.value || !this.validateCalendar(input.value)) {
      const today = new Date();
      this.addContractForm.patchValue({ [controlName]: today });
    }
  }

  validateCalendar(dateString: string): boolean {
    return !isNaN(Date.parse(dateString));
  }

  initFormHopDong(): void {
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

        tienDoHopDong: [{ value: null, disabled: true }],
        ngayKetThucHopDong: [{ value: null, disabled: true }],
        giaTriHopDongBanDau: [{ value: null, disabled: true }],
        giaTriThucTe: [{ value: null, disabled: true }],
        daThanhToan: [{ value: null, disabled: true }],
        conThanhToan: [{ value: null, disabled: true }],
        ghiChu: [null, Validators.maxLength(500)],
      },
      { validators: this.validateNgayCoHieuLuc }
    );
  }

  initFormHangHoa(): void {
    this.rowControls = this.listChiTietHangHoa.map((item) => {
      const soLuongControl = new FormControl(null, [
        Validators.required,
        Validators.maxLength(50),
      ]);
      const donGiaControl = new FormControl(null, [
        Validators.required,
        Validators.maxLength(50),
      ]);

      const controlGroup = {
        namSanXuatControl: new FormControl(null, [
          Validators.required,
          Validators.min(2000),
        ]),
        modelControl: new FormControl(null, [
          Validators.required,
          Validators.maxLength(50),
        ]),
        nguonKinhPhiControl: new FormControl(null, Validators.required),
        hangSanXuatControl: new FormControl(null, Validators.required),
        xuatXuControl: new FormControl(null, Validators.required),

        soLuongControl: soLuongControl,
        donGiaControl: donGiaControl,
        total: 0,
      };

      soLuongControl.valueChanges.subscribe(() => {
        this.updateThanhTien(controlGroup);
      });
      donGiaControl.valueChanges.subscribe(() => {
        this.updateThanhTien(controlGroup);
      });

      return controlGroup;
    });
  }

  isControlInvalid(index: number, controlName: string): boolean {
    const control = this.rowControls[index][controlName] as FormControl;
    const isInvalid = control.invalid && (control.dirty || control.touched);
    this.cdr.markForCheck(); // Buộc cập nhật giao diện
    return isInvalid;
  }

  validateAllControls(): boolean {
    let allValid = true;
    this.rowControls.forEach((controlGroup) => {
      Object.values(controlGroup).forEach((control: any) => {
        if (control.invalid) {
          allValid = false;
        }
      });
    });
    this.cdr.markForCheck(); // Cập nhật giao diện sau khi đánh dấu tất cả controls
    return allValid;
  }

  getListDuLieuHangHoa(): any[] {
    return this.rowControls.map((controlGroup) => ({
      nguonKinhPhi: controlGroup.nguonKinhPhiControl.value,
      hangSanXuat: controlGroup.hangSanXuatControl.value,
      xuatXu: controlGroup.xuatXuControl.value,
      namSanXuat: controlGroup.namSanXuatControl.value,
      model: controlGroup.modelControl.value,
      soLuong: controlGroup.soLuongControl.value,
      donGia: controlGroup.donGiaControl.value,
    }));
  }

  formatThanhTien(value: number): string {
    return value ? value.toLocaleString('vi-VN') : '0';
  }

  updateThanhTien(controlGroup: any): void {
    const soLuong = controlGroup.soLuongControl.value || 0;
    const donGia = controlGroup.donGiaControl.value || 0;
    controlGroup.total = soLuong * donGia;
  }

  loadMockData(): void {
    this.listHangHoa = Array.from({ length: 100 }, (_, i) => ({
      id: i,
      code: `P00${i}`,
      name: `Sản phẩm ${i}`,
      category: `Loại ${i}`,
      quantity: 100 + i,
    }));
    // this.listChiTietHangHoa = Array.from({ length: 1 }, (_, i) => ({
    //   id: i,
    //   code: `P00${i}`,
    //   name: `Sản phẩm ${i}`,
    //   category: `Loại ${i}`,
    //   quantity: 100 + i,
    // }));
  }

  validateNgayCoHieuLuc(group: FormGroup): ValidationErrors | null {
    const ngayKy = group.get('ngayKy')?.value;
    const ngayCoHieuLuc = group.get('ngayCoHieuLuc')?.value;
    if (ngayKy && ngayCoHieuLuc && new Date(ngayKy) > new Date(ngayCoHieuLuc)) {
      return { dateError: 'Phải lớn hơn hoặc bằng ngày ký.' };
    }
    if (ngayKy == null && ngayCoHieuLuc) {
      return { dateError: 'Phải lớn hơn hoặc bằng ngày ký.' };
    }
    return null;
  }

  updateNgayKetThucHopDong(): void {
    const ngayCoHieuLuc = this.addContractForm.get('ngayCoHieuLuc')?.value;
    const soNgayThucHien = this.addContractForm.get('soNgayThucHien')?.value;

    if (ngayCoHieuLuc) {
      if (soNgayThucHien > 0) {
        const ngayKetThuc = new Date(ngayCoHieuLuc);
        ngayKetThuc.setDate(ngayKetThuc.getDate() + Number(soNgayThucHien));
        this.addContractForm.get('ngayKetThucHopDong')?.setValue(ngayKetThuc);
      } else {
        this.addContractForm.get('ngayKetThucHopDong')?.setValue(ngayCoHieuLuc);
      }
      this.addContractForm.get('ngayKetThucHopDong')?.markAsTouched();
      this.addContractForm.get('ngayKetThucHopDong')?.updateValueAndValidity();
    }
  }
}
