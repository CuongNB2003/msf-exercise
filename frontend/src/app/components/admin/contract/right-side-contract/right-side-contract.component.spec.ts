import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RightSideContractComponent } from './right-side-contract.component';

describe('RightSideContractComponent', () => {
  let component: RightSideContractComponent;
  let fixture: ComponentFixture<RightSideContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RightSideContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RightSideContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
