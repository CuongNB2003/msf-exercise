import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PrimeModule } from '@modules/prime/prime.module';
@Component({
  selector: 'app-home-admin',
  standalone: true,
  imports: [PrimeModule, ],
  templateUrl: './home-admin.component.html',
  styleUrl: './home-admin.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HomeAdminComponent implements OnInit {
  contractForm!: FormGroup;
  viLocale: any;
  isActive: boolean = false;
  countries: any[] | undefined;
  selectedCountry: string | undefined;
  statusOptions = [
    { name: 'Lưu tạm', value: 'LUU_TAM' },
    { name: 'Chính thức', value: 'CHINH_THUC' },
    { name: 'Tất cả', value: 'TAT_CA' }
  ];
  selectedStatus: string = '';

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.countries = [
      { name: 'Australia', code: 'AU' },
      { name: 'Brazil', code: 'BR' },
      { name: 'China', code: 'CN' },
      { name: 'Egypt', code: 'EG' },
      { name: 'France', code: 'FR' },
      { name: 'Germany', code: 'DE' },
      { name: 'India', code: 'IN' },
      { name: 'Japan', code: 'JP' },
      { name: 'Spain', code: 'ES' },
      { name: 'United States', code: 'US' },
      { name: 'Việt Nam', code: 'VN' },
    ];

    this.contractForm = this.fb.group({
      namKinhPhi: ['', [Validators.required, Validators.maxLength(4)]],
      soHopDong: ['', [Validators.required, Validators.maxLength(50)]],
      loaiHopDong: [null, Validators.required],
      tenHopDong: ['', Validators.maxLength(250)],
      trangThai: [null],
      donViCungCap: [null],
      tuNgay: [null],
      denNgay: [null],
      lanhDaoPhuTrach: [null],
      canBoPhuTrach: [null],
      tenHang: ['', Validators.maxLength(200)],
      maHang: ['', Validators.maxLength(200)],
      tienDoHopDong: [null]
    });
  }

  onSubmit() {
    if (this.contractForm.valid) {
      const formData = this.contractForm.value;
      console.log(formData);
    }
  }

  toggleClass() {
    this.isActive = !this.isActive;
  }

  toUpperCase(event: any): void {
    event.target.value = event.target.value.toUpperCase();
  }

  handleEnter(event: any): void {
    const inputValue = event.target.value;
    if (!inputValue || !this.isValidDate(inputValue)) {
      const today = new Date();
      const formattedDate = this.formatDate(today);
      event.target.value = formattedDate;
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
