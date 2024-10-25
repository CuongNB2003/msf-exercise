import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutContractComponent } from './layout-contract.component';

describe('LayoutContractComponent', () => {
  let component: LayoutContractComponent;
  let fixture: ComponentFixture<LayoutContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LayoutContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
