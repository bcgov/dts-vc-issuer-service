import { TestBed } from '@angular/core/testing';

import { ErrorHandlerInterceptor } from './error-handler.interceptor';

describe('ErrorHandlerInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: []
  }));

  it('should create', () => {
    const service: ErrorHandlerInterceptor = TestBed.inject(ErrorHandlerInterceptor);
    expect(service).toBeTruthy();
  });
});
