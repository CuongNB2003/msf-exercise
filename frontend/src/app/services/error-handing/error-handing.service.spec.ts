import { TestBed } from '@angular/core/testing';

import { ErrorHandingService } from './error-handing.service';

describe('ErrorHandingService', () => {
  let service: ErrorHandingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ErrorHandingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
