import { TestBed } from '@angular/core/testing';

import { IssuerResource } from './issuer-resource.service';

describe('IssuerResource', () => {
  let service: IssuerResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IssuerResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
