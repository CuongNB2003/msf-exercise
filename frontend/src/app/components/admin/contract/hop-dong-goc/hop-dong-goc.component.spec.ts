import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HopDongGocComponent } from './hop-dong-goc.component';

describe('HopDongGocComponent', () => {
  let component: HopDongGocComponent;
  let fixture: ComponentFixture<HopDongGocComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HopDongGocComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HopDongGocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
