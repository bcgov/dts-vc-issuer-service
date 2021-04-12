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

  public constructor(
    private immunizationResource: ImmunizationResource,
    private issuerResource: IssuerResource
  ) {
    this.immunizationRecords = null;
    this.patient = null;
    this.issuedCredential = null;
  }

  public ngOnInit(): void {
    const patientId = '9039555099';
    this.immunizationResource.immunizations(patientId)
      .subscribe((immunizationRecords: ImmunizationRecord[]) => this.immunizationRecords = immunizationRecords);

    const patient = {
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
    };
    this.issuerResource.createPatient(patient)
      .pipe(
        map((patient: Patient) => this.patient = patient),
        exhaustMap((patient: Patient) =>
          this.immunizationResource.immunizations('9039555099')
            .pipe(map((immunizationRecords: ImmunizationRecord[]) => [patient.id, immunizationRecords]))
        ),
        exhaustMap((params: [number, ImmunizationRecord[]]) => this.issuerResource.issueCredential(...params))
      )
      .subscribe((issuedCredential: string) => this.issuedCredential = issuedCredential);
  }
}
