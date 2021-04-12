import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';

import { AppConfig, APP_CONFIG } from 'app/app-config.module';
import { ApiResourceUtilsService } from '@core/resources/api-resource-utils.service';
import { LoggerService } from '@core/services/logger.service';

import { ImmunizationRecord } from '../models/immunization-record.model';

@Injectable({
  providedIn: 'root'
})
export class ImmunizationResource {

  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private http: HttpClient,
    private logger: LoggerService,
    private apiResourceUtilsService: ApiResourceUtilsService
  ) { }

  public immunizations(patientId: string): Observable<ImmunizationRecord[]> {
    const params = this.apiResourceUtilsService.makeHttpParams({ patientId });
    return this.http.get<ImmunizationRecord[]>(`${ this.config.apiEndpoints.immunization }/immunization`, { params })
      .pipe(
        map((immunizations: ImmunizationRecord[]) => (immunizations) ? immunizations : []),
        tap((immunizations: ImmunizationRecord[]) => this.logger.info('IMMUNIZATIONS', immunizations)),
        catchError((error: any) => {
          this.logger.error('ImmunizationResource::immunizations error has occurred: ', error);
          throw error;
        })
      );
  }

  public immunization(immunizationId: number): Observable<ImmunizationRecord> {
    return this.http.get<ImmunizationRecord>(`${ this.config.apiEndpoints.immunization }/immunization/${ immunizationId }`)
      .pipe(
        tap((immunization) => this.logger.info('IMMUNIZATION', immunization)),
        catchError((error: any) => {
          this.logger.error('ImmunizationResource::immunization error has occurred: ', error);
          throw error;
        })
      );
  }
}
