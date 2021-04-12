import { TestBed } from '@angular/core/testing';

import { ImmunizationResource } from './immunization-resource.service';

describe('ImmunizationResource', () => {
  let service: ImmunizationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmunizationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
