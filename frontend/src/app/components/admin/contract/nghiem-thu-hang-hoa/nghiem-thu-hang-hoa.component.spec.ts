import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NghiemThuHangHoaComponent } from './nghiem-thu-hang-hoa.component';

describe('NghiemThuHangHoaComponent', () => {
  let component: NghiemThuHangHoaComponent;
  let fixture: ComponentFixture<NghiemThuHangHoaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NghiemThuHangHoaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NghiemThuHangHoaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
