import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionListComponent } from './permission-list.component';

describe('PermissionListComponent', () => {
  let component: PermissionListComponent;
  let fixture: ComponentFixture<PermissionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissionListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
