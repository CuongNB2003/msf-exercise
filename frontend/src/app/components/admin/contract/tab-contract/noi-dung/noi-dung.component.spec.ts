import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoiDungComponent } from './noi-dung.component';

describe('NoiDungComponent', () => {
  let component: NoiDungComponent;
  let fixture: ComponentFixture<NoiDungComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoiDungComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NoiDungComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
