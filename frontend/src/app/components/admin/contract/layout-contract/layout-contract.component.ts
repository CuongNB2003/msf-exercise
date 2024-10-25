import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { LeftSideContractComponent } from '../left-side-contract/left-side-contract.component';
import { RightSideContractComponent } from '../right-side-contract/right-side-contract.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-layout-contract',
  standalone: true,
  imports: [LeftSideContractComponent, RightSideContractComponent, CommonModule],
  templateUrl: './layout-contract.component.html',
  styleUrl: './layout-contract.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LayoutContractComponent {
  isActive: boolean = false;
}
