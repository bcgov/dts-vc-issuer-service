import { TestBed } from '@angular/core/testing';

import { IssuerGuard } from './issuer.guard';

describe('IssuerGuard', () => {
  let guard: IssuerGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(IssuerGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
