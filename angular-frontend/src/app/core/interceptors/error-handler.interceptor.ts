import { Injectable } from '@angular/core';
import { HttpHandler, HttpRequest, HttpEvent, HttpErrorResponse } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { LoggerService } from '@core/services/logger.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerInterceptor {
  public constructor(
    private logger: LoggerService
  ) { }

  /**
   * @description
   * Intercept error responses.
   */
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const status = error.status;
          switch (status) {
            case 400:
              this.logger.error('Bad Request', error);
              break;
            case 401:
              this.logger.error('Unauthorized', error);
              break;
            case 403:
              this.logger.error('Forbidden', error);
              break;
            case 404:
              this.logger.error('Not Found', error);
              break;
            case 422:
              this.logger.error('Unprocessable Entity', error);
              break;
            case 500:
              this.logger.error('Internal Server Error', error);
              break;
            default:
              this.logger.error('Unhandled HTTP response.', error);
              break;
          }

          return throwError(error);
        })
      );
  }
}
