import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TienDoThanhToanComponent } from './tien-do-thanh-toan.component';

describe('TienDoThanhToanComponent', () => {
  let component: TienDoThanhToanComponent;
  let fixture: ComponentFixture<TienDoThanhToanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TienDoThanhToanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TienDoThanhToanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
