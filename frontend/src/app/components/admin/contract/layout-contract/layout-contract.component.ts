import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { LeftSideContractComponent } from '../left-side-contract/left-side-contract.component';
import { CommonModule } from '@angular/common';
import { PrimeModule } from '@modules/prime/prime.module';
import { PhuLucComponent } from '../phu-luc/phu-luc.component';
import { NoiDungComponent } from '../noi-dung/noi-dung.component';

@Component({
  selector: 'app-layout-contract',
  standalone: true,
  imports: [
    LeftSideContractComponent, 
    CommonModule,
    PhuLucComponent,
    NoiDungComponent,
    PrimeModule
  ],
  templateUrl: './layout-contract.component.html',
  styleUrl: './layout-contract.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LayoutContractComponent {
  isActive: boolean = false;
  isAdd: boolean = true

  toggleClass(): void {
    this.isActive = !this.isActive;
  }

  onSubmit(): void {
    this.isAdd = !this.isAdd;
  }

  
}
