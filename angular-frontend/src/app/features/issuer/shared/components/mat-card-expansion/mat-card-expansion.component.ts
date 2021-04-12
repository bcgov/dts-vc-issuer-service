import { Component, Input } from '@angular/core';

import { ImmunizationRecord } from '../../models/immunization-record.model';

@Component({
  selector: 'app-mat-card-expansion',
  templateUrl: './mat-card-expansion.component.html',
  styleUrls: ['./mat-card-expansion.component.scss']
})
export class MatCardExpansionComponent {
  @Input() public immunizationRecord!: ImmunizationRecord;
  public expanded: boolean;

  public constructor() {
    this.expanded = false;
  }
}
