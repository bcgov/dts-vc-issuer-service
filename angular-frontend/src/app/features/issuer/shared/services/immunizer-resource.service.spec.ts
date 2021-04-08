import { TestBed } from '@angular/core/testing';

import { ImmunizerResource } from './immunizer-resource.service';

describe('ImmunizerResource', () => {
  let service: ImmunizerResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmunizerResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
