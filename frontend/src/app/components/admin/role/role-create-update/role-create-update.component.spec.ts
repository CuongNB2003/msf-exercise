import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleCreateUpdateComponent } from './role-create-update.component';

describe('RoleCreateUpdateComponent', () => {
  let component: RoleCreateUpdateComponent;
  let fixture: ComponentFixture<RoleCreateUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoleCreateUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleCreateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
