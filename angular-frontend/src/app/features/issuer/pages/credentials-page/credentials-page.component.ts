import { Component, OnInit } from '@angular/core';

import { exhaustMap, map } from 'rxjs/operators';

import { ImmunizationRecord } from '@features/issuer/shared/models/immunization-record.model';
import { Patient } from '@features/issuer/shared/models/patient.model';
import { ImmunizationResource } from '@features/issuer/shared/services/immunization-resource.service';
import { IssuerResource } from '@features/issuer/shared/services/issuer-resource.service';

@Component({
  selector: 'app-credentials-page',
  templateUrl: './credentials-page.component.html',
  styleUrls: ['./credentials-page.component.scss']
})
export class CredentialsPageComponent implements OnInit {
  public immunizationRecords: ImmunizationRecord[] | null;
  public patient: Patient | null;
  public issuedCredential: string | null;

  public selectedImmunizationRecords: ImmunizationRecord[];

  private readonly patientSeed = {
    patientId: '9039555099',
    create: {
      userId: '22091b5c-b2df-4f6e-b184-46d7bee84b08', // GUID
      hpdid: '22091b5c-b2df-4f6e-b184-46d7bee84b08',
      firstName: 'Foghorn',
      lastName: 'Leghorn',
      givenNames: 'Foghorn Leghorn',
      preferredFirstName: '',
      preferredMiddleName: '',
      preferredLastName: '',
      dateOfBirth: '2021-09-22',
      email: 'foghorn.leghorn@example.com',
      phone: '9999999999'
    }
  };

  public constructor(
    private immunizationResource: ImmunizationResource,
    private issuerResource: IssuerResource
  ) {
    this.immunizationRecords = null;
    this.patient = null;
    this.issuedCredential = null;
    this.selectedImmunizationRecords = [];
  }

  public addSelectedImmunizationRecord(immunizationRecord: ImmunizationRecord) {
    if (!this.selectedImmunizationRecords.some(ir => ir.id === immunizationRecord.id)) {
      this.selectedImmunizationRecords.push(immunizationRecord);
    }
  }

  public removeSelectedImmunizationRecord(immunizationId: string) {
    const index = this.selectedImmunizationRecords.findIndex(ir => ir.id === immunizationId);
    if (index > -1) {
      this.selectedImmunizationRecords.splice(index, 1);
    }
  }

  public onCreateCredential() {
    this.issuerResource.issueCredential(this.patient.id, this.selectedImmunizationRecords)
      .subscribe((issuedCredential: string) => this.issuedCredential = issuedCredential);
  }

  public ngOnInit(): void {
    this.immunizationResource.immunizations(this.patientSeed.patientId)
      .subscribe((immunizationRecords: ImmunizationRecord[]) =>
        this.immunizationRecords = immunizationRecords
      );

    // TODO check for existence of the patient before creation
    this.issuerResource.createPatient(this.patientSeed.create)
      .pipe(map((patient: Patient) => this.patient = patient))
      .subscribe();
  }
}
