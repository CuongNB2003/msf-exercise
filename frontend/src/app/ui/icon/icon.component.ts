import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { MaterialModule } from '../../modules/material/material.module';

@Component({
  selector: 'app-icon',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './icon.component.html',
  styleUrl: './icon.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class IconComponent implements OnInit {
  icons: string[] = []; // Mảng chứa danh sách icon
  @Input() selectedIcon: string | null = null;
  @Output() iconSelected = new EventEmitter();

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<{ icons: { name: string }[] }>('icons.json')
      .subscribe({
        next: (data) => {
          this.icons = data.icons.map(icon => icon.name);
        },
        error: (err) => {
          console.error('Có lỗi xảy ra khi lấy danh sách icon:', err);
        }
      });
  }

  selectIcon(icon: string): void {
    this.selectedIcon = icon; // Cập nhật icon đã chọn
    this.iconSelected.emit(icon); // Gửi icon đã chọn ra ngoài component
  }

  // Hàm xóa icon đã chọn
  clearIcon() {
    this.selectedIcon = ''; // Xóa biểu tượng đã chọn
    this.iconSelected.emit(this.selectedIcon); // Gửi thông tin xóa ra ngoài component
  }
}
