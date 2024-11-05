import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThanhLyHopDongComponent } from './thanh-ly-hop-dong.component';

describe('ThanhLyHopDongComponent', () => {
  let component: ThanhLyHopDongComponent;
  let fixture: ComponentFixture<ThanhLyHopDongComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThanhLyHopDongComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThanhLyHopDongComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
