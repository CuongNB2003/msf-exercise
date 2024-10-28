import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-right-side-contract',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './right-side-contract.component.html',
  styleUrl: './right-side-contract.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RightSideContractComponent {
  isAdd: boolean = true

  onSubmit(): void {
    this.isAdd = !this.isAdd;
  }
}
