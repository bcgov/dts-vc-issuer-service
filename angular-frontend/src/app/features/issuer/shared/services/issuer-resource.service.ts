import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

import { AppConfig, APP_CONFIG } from 'app/app-config.module';
import { LoggerService } from '@core/services/logger.service';

import { Patient } from '../models/patient.model';
import { ImmunizationRecord } from '../models/immunization-record.model';
import { IssuedCredential } from '../models/issued-credential.model';

@Injectable({
  providedIn: 'root'
})
export class IssuerResource {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private http: HttpClient,
    private logger: LoggerService
  ) { }

  /**
   * @description
   * Create a new patient profile.
   */
  public createPatient(patient: Patient): Observable<Patient> {
    return this.http.post<Patient>(`${ this.config.apiEndpoints.issuer }/patients`, patient)
      .pipe(
        tap((patient: Patient) => this.logger.info('PATIENT', patient)),
        catchError((error: any) => {
          this.logger.error('IssuerResource::createPatient error has occurred: ', error);
          throw error;
        })
      );
  }

  /**
   * @description
   * Get the user by the unique ID associated with their profile.
   */
  public getPatientByPatientId(patientId: number): Observable<Patient> {
    return this.http.get<Patient>(`${ this.config.apiEndpoints.issuer }/patients/${ patientId }`)
      .pipe(
        tap((patient: Patient) => this.logger.info('PATIENT', patient)),
        catchError((error: any) => {
          this.logger.error('IssuerResource::getPatientByPatientId error has occurred: ', error);
          throw error;
        })
      );
  }

  /**
   * @description
   * Get the patient by the user ID provided by the authentication token.
   */
  public getPatientByUserId(userId: string): Observable<Patient> {
    return this.http.get<Patient>(`${ this.config.apiEndpoints.issuer }/patients/${ userId }`)
      .pipe(
        tap((patient: Patient) => this.logger.info('PATIENT', patient)),
        catchError((error: any) => {
          this.logger.error('IssuerResource::getPatientByUserId error has occurred: ', error);
          throw error;
        })
      );
  }

  /**
   * @description
   * Issue a credential for an existing patient.
   */
  public issueCredential(patientId: number, immunizationRecords: ImmunizationRecord[]): Observable<string> {
    const payload = immunizationRecords.map(({ id: guid, fullUrl: uri }: ImmunizationRecord) => ({ guid, uri }));
    return this.http.post<string>(`${ this.config.apiEndpoints.issuer }/patients/${ patientId }/credential`, payload)
      .pipe(
        tap((credential: string) => this.logger.info('CREDENTIAL', credential)),
        catchError((error: any) => {
          this.logger.error('IssuerResource::issueCredential error has occurred: ', error);
          throw error;
        })
      );
  }
}
