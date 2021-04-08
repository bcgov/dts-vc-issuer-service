import { Injectable } from '@angular/core';

import { LoggerService } from '@core/services/logger.service';

@Injectable({
  providedIn: 'root'
})
export class ImmunizerResource {
  public constructor(
    // private apiResource: ApiResource,
    // private apiResourceUtilsService: ApiResourceUtilsService,
    // private toastService: ToastService,
    private logger: LoggerService
  ) { }
}
