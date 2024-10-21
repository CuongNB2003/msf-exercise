import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { menuGuard } from './menu.guard';

describe('menuGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => menuGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
