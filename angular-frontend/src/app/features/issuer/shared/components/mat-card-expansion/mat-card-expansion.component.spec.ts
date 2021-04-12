import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatCardExpansionComponent } from './mat-card-expansion.component';

describe('MatCardExpansionComponent', () => {
  let component: MatCardExpansionComponent;
  let fixture: ComponentFixture<MatCardExpansionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MatCardExpansionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MatCardExpansionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
