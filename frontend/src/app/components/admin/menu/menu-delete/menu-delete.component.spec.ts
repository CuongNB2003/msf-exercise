import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuDeleteComponent } from './menu-delete.component';

describe('MenuDeleteComponent', () => {
  let component: MenuDeleteComponent;
  let fixture: ComponentFixture<MenuDeleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MenuDeleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MenuDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
