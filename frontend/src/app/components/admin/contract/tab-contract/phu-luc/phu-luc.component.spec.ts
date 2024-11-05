import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhuLucComponent } from './phu-luc.component';

describe('PhuLucComponent', () => {
  let component: PhuLucComponent;
  let fixture: ComponentFixture<PhuLucComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PhuLucComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhuLucComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
