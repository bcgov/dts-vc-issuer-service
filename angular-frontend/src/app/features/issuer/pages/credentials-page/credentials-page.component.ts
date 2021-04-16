import { Component, OnInit } from '@angular/core';

import { forkJoin, of, Subscription } from 'rxjs';
import { exhaustMap } from 'rxjs/operators';

import { AuthService } from '@core/services/auth.service';

import { ImmunizationRecord } from '@features/issuer/shared/models/immunization-record.model';
import { Patient } from '@features/issuer/shared/models/patient.model';
import { ImmunizationResource } from '@features/issuer/shared/services/immunization-resource.service';
import { IssuerResource } from '@features/issuer/shared/services/issuer-resource.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertOptions } from '@shared/components/card-alert/card-alert.component';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-credentials-page',
  templateUrl: './credentials-page.component.html',
  styleUrls: ['./credentials-page.component.scss']
})
export class CredentialsPageComponent implements OnInit {
  public busy: Subscription;
  public title: string;
  public immunizationRecords: ImmunizationRecord[] | null;
  public patient: Patient | null;
  public issuedCredential: string | null;

  public selectedImmunizationRecords: ImmunizationRecord[];
  public savedImmunizationRecords: ImmunizationRecord[];

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private immunizationResource: ImmunizationResource,
    private issuerResource: IssuerResource,
    private authService: AuthService,
    private translocoService: TranslocoService
  ) {
    this.title = '';
    this.immunizationRecords = null;
    this.patient = null;
    this.issuedCredential = null;
    this.selectedImmunizationRecords = [];
    this.savedImmunizationRecords = [];
  }

  public get username(): string {
    return this.authService.token.fullName;
  }

  public getAlertOptions(immunizationRecord: ImmunizationRecord): AlertOptions {
    // TODO temporary way of managing record state indicators
    // TODO MVP demo does not know if it already exists in the digital wallet
    // TODO not managing danger notice until after MVP demo
    // TODO only managing success notice on submission for MVP demo
    // TODO danger was not added as it identicates that something is wrong instead of messaging that you could do something

    const isUnsaved = this.immunizationRecords.some(ir => ir.id === immunizationRecord.id);
    const isPending = this.selectedImmunizationRecords.some(ir => ir.id === immunizationRecord.id);
    const isSaved = this.savedImmunizationRecords.some(ir => ir.id === immunizationRecord.id);

    if (isUnsaved && isPending) {
      return {
        type: 'warn',
        icon: 'task_alt',
        message: this.translocoService.translate('vaccinationRecordReadyToBeSavedInWallet'),
        showAction: true,
        disableAction: true
      };
    } else if (isSaved) {
      return {
        type: 'success',
        icon: 'verified',
        message: this.translocoService.translate('vaccinationRecordExistsInWallet'),
        showAction: false
      };
    } else {
      return {
        type: 'info',
        icon: 'notification_important',
        message: this.translocoService.translate('notAddedToCertificateMessage'),
        showAction: true,
        disableAction: isPending
      };
    }
  }

  /**
   * @description
   * Add all the immunization records that do not already
   * exist to the "cart".
   */
  public addAllImmunizationRecords(): void {
    this.selectedImmunizationRecords = [...this.immunizationRecords];
  }

  /**
   * @description
   * Add selected immunization records that do not already
   * exist to the "cart".
   */
  public addSelectedImmunizationRecord(immunizationRecord: ImmunizationRecord): void {
    if (!this.selectedImmunizationRecords.some(ir => ir.id === immunizationRecord.id)) {
      this.selectedImmunizationRecords.push(immunizationRecord);
    }
  }

  /**
   * @description
   * Remove a selected immunization record from the "cart".
   */
  public removeSelectedImmunizationRecord(immunizationId: string): void {
    const index = this.selectedImmunizationRecords.findIndex(ir => ir.id === immunizationId);
    if (index > -1) {
      this.selectedImmunizationRecords.splice(index, 1);
    }
  }

  /**
   * @description
   * Handle the creation of a credential.
   */
  public onCreateCredential(): void {
    this.busy = this.issuerResource.issueCredential(this.patient.id, this.selectedImmunizationRecords)
      .subscribe((issuedCredential: string) => {
        this.issuedCredential = issuedCredential;
        this.savedImmunizationRecords.push(...this.selectedImmunizationRecords);
        this.selectedImmunizationRecords = [];
      });
  }

  /**
   * @description
   * Handle logout.
   */
  public onLogout(): void {
    this.router.navigate(['/login']);
  }

  public ngOnInit(): void {
    const patientId = this.authService.token.userId;

    const immunizations$ = this.immunizationResource.immunizations(patientId);
    const patient$ = this.issuerResource.getPatientByUserId(patientId)
      .pipe(
        exhaustMap((patient: Patient) =>
          (!patient)
            ? this.issuerResource.createPatient(this.authService.token)
            : of(patient)
        )
      );

    this.busy = forkJoin({
      immunizationRecords: immunizations$,
      patient: patient$
    }).subscribe((response: { immunizationRecords: ImmunizationRecord[], patient: Patient; }) => {
      this.immunizationRecords = response.immunizationRecords;
      this.patient = response.patient;
    });
  }
}
