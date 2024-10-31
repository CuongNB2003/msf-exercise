import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-noi-dung',
  standalone: true,
  imports: [PrimeModule, CommonModule],
  templateUrl: './noi-dung.component.html',
  styleUrl: './noi-dung.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class NoiDungComponent implements OnInit {
  addContractForm!: FormGroup
  products: any[] = []
  selectedProducts!: any;
  countries = [
    { name: 'Việt Nam', code: 'VN' },
    { name: 'Cường Đây rồi', code: 'VN' },
    { name: 'HIHI', code: 'VN' }
  ];

  listStatus = [
    { name: 'Lưu tạm', value: 'LUU_TAM' },
    { name: 'Chính thức', value: 'CHINH_THUC' }
  ];

  listProgress = [
    { name: 'Chưa nghiệm thu', value: 'LUU_TAM' },
    { name: 'Nghiệm thu một phần', value: 'CHINH_THUC' },
    { name: 'Hoàn thành nghiệm thu', value: 'TAT_CA' },
    { name: 'Thanh lý', value: 'TAT_CA' },
    { name: 'Quyết toán', value: 'TAT_CA' }
  ];

  constructor(private fb: FormBuilder) {
    this.products = [
      { code: 'P004', name: 'Sản phẩm D Sản phẩm D Sản phẩm D Sản phẩm D Sản phẩm D Sản phẩm D Sản phẩm D', category: 'Loại 1', quantity: 75 },
      { code: 'P005', name: 'Sản phẩm E', category: 'Loại 2', quantity: 120 },
      { code: 'P005', name: 'Sản phẩm E', category: 'Loại 2', quantity: 120 }
    ];
  }

  ngOnInit(): void {
    this.addContractForm = this.fb.group({
      loaiHopDong: [null, Validators.required],
      namKinhPhi: [null, [Validators.required, Validators.maxLength(4)]],
      canBoPhuTrach: [null, Validators.required],
      lanhDaoPhuTrach: [null],
      trangThai: [null, Validators.required],
      tienDoHopDong: [{ value: '', disabled: true }],
      soHopDong: [null, [Validators.required, Validators.maxLength(50)]],
      tenHopDong: [null, Validators.maxLength(250)],
      nguonKinhPhi: [null],
      donViCungCap: [null, Validators.required],
      ngayKy: [null, Validators.required],
      ngayCoHieuLuc: [null],
      soNgayThucHien: [null, [Validators.required, Validators.maxLength(5)]],
      ngayKetThucHopDong: [{ value: null, disabled: true }],
      giaTriHopDongBanDau: [{ value: null, disabled: true }],
      giaTriThucTe: [{ value: null, disabled: true }],
      daThanhToan: [{ value: null, disabled: true }],
      conThanhToan: [{ value: null, disabled: true }],
      ghiChu: [null, Validators.maxLength(500)],
    });
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
}
