import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuCreateUpdateComponent } from './menu-create-update.component';

describe('MenuCreateUpdateComponent', () => {
  let component: MenuCreateUpdateComponent;
  let fixture: ComponentFixture<MenuCreateUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuCreateUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MenuCreateUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
