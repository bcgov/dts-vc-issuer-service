import { NgModule, Optional, SkipSelf, ErrorHandler } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { throwIfAlreadyLoaded } from '@core/module-import-guard';

import { ErrorHandlerService } from '@core/services/error-handler.service';
import { ErrorHandlerInterceptor } from '@core/interceptors/error-handler.interceptor';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: ErrorHandlerService
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerInterceptor,
      multi: true
    }
  ],
  exports: [
    BrowserModule
  ]
})
export class CoreModule {
  public constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
