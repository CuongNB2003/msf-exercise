import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionCreateUpdateComponent } from './permission-create-update.component';

describe('PermissionCreateUpdateComponent', () => {
  let component: PermissionCreateUpdateComponent;
  let fixture: ComponentFixture<PermissionCreateUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissionCreateUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionCreateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
