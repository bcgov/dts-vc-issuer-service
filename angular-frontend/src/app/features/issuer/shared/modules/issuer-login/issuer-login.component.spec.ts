import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssuerLoginComponent } from './issuer-login.component';

describe('IssuerLoginComponent', () => {
  let component: IssuerLoginComponent;
  let fixture: ComponentFixture<IssuerLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [IssuerLoginComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IssuerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
