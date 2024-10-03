import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarIconComponent } from './sidebar-icon.component';

describe('SidebarLayoutComponent', () => {
  let component: SidebarIconComponent;
  let fixture: ComponentFixture<SidebarIconComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidebarIconComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(SidebarIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
