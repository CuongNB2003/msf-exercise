import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionDeleteComponent } from './permission-delete.component';

describe('PermissionDeleteComponent', () => {
  let component: PermissionDeleteComponent;
  let fixture: ComponentFixture<PermissionDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissionDeleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
