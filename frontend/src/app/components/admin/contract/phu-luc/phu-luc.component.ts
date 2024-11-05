import { map } from 'rxjs';
import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { PrimeModule } from '@modules/prime/prime.module';

@Component({
  selector: 'app-phu-luc',
  standalone: true,
  imports: [PrimeModule],
  templateUrl: './phu-luc.component.html',
  styleUrl: './phu-luc.component.scss',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class PhuLucComponent implements OnInit {
  listPhuLuc: any[] = [];

  ngOnInit(): void {}
}
