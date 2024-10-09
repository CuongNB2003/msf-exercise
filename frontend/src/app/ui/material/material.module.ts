import { NgModule } from '@angular/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete'; // Tự động hoàn thành
import { MatCheckboxModule } from '@angular/material/checkbox'; // Hộp kiểm
import { MatDatepickerModule } from '@angular/material/datepicker'; // Bộ chọn ngày
import { MatFormFieldModule } from '@angular/material/form-field'; // Trường biểu mẫu
import { MatInputModule } from '@angular/material/input'; // Trường nhập liệu
import { MatRadioModule } from '@angular/material/radio'; // Nút radio
import { MatSelectModule } from '@angular/material/select'; // Hộp chọn
import { MatSliderModule } from '@angular/material/slider'; // Thanh trượt
import { MatSlideToggleModule } from '@angular/material/slide-toggle'; // Công tắc trượt
import { MatMenuModule } from '@angular/material/menu'; // Menu
import { MatSidenavModule } from '@angular/material/sidenav'; // Thanh điều hướng bên
import { MatToolbarModule } from '@angular/material/toolbar'; // Thanh công cụ
import { MatCardModule } from '@angular/material/card'; // Thẻ
import { MatDividerModule } from '@angular/material/divider'; // Đường phân cách
import { MatExpansionModule } from '@angular/material/expansion'; // Bảng mở rộng
import { MatGridListModule } from '@angular/material/grid-list'; // Danh sách lưới
import { MatListModule } from '@angular/material/list'; // Danh sách
import { MatStepperModule } from '@angular/material/stepper'; // Bước
import { MatButtonModule } from '@angular/material/button'; // Nút
import { MatButtonToggleModule } from '@angular/material/button-toggle'; // Nút chuyển đổi
import { MatBadgeModule } from '@angular/material/badge'; // Huy hiệu
import { MatChipsModule } from '@angular/material/chips'; // Chip
import { MatIconModule } from '@angular/material/icon'; // Biểu tượng
import { MatProgressBarModule } from '@angular/material/progress-bar'; // Thanh tiến trình
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'; // Vòng quay tiến trình
import { MatRippleModule } from '@angular/material/core'; // Hiệu ứng gợn sóng
import { MatDialogModule } from '@angular/material/dialog'; // Hộp thoại
import { MatSnackBarModule } from '@angular/material/snack-bar'; // Thông báo nhanh
import { MatTooltipModule } from '@angular/material/tooltip'; // Chú giải công cụ
import { MatPaginatorModule } from '@angular/material/paginator'; // Bộ phân trang
import { MatSortModule } from '@angular/material/sort'; // Sắp xếp
import { MatTableModule } from '@angular/material/table'; // Bảng dữ liệu
import { MatBottomSheetModule } from '@angular/material/bottom-sheet'; // Tấm dưới cùng
import { MatTreeModule } from '@angular/material/tree'; // Cây
import { MatTabsModule } from '@angular/material/tabs'; // Tabs

@NgModule({
  exports: [
    MatAutocompleteModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatListModule,
    MatStepperModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatBadgeModule,
    MatChipsModule,
    MatIconModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatBottomSheetModule,
    MatTreeModule,
    MatTabsModule
  ]
})
export class MaterialModule { }
