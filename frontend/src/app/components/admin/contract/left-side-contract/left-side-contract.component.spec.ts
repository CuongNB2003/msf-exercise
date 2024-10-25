import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftSideContractComponent } from './left-side-contract.component';

describe('LeftSideContractComponent', () => {
  let component: LeftSideContractComponent;
  let fixture: ComponentFixture<LeftSideContractComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftSideContractComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeftSideContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
