import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChiTietHangHoaNghiemThuComponent } from './chi-tiet-hang-hoa-nghiem-thu.component';

describe('ChiTietHangHoaNghiemThuComponent', () => {
  let component: ChiTietHangHoaNghiemThuComponent;
  let fixture: ComponentFixture<ChiTietHangHoaNghiemThuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChiTietHangHoaNghiemThuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChiTietHangHoaNghiemThuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
