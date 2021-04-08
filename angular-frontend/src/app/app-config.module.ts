import { NgModule, InjectionToken } from '@angular/core';

import { AppRoutes } from './app.routes';

import { environment } from '@env/environment';

import { AuthRoutes } from '@features/auth/auth.routes';
import { IssuerRoutes } from '@features/issuer/issuer.routes';

export let APP_CONFIG = new InjectionToken<AppConfig>('app.config');

export interface AppConfig {
  apiEndpoints: {
    immunization: string;
    issuer: string;
  };
  loginRedirectUrl: string;
  routes: {
    denied: string;
    auth: string;
    issuer: string;
  };
}

export const APP_DI_CONFIG: AppConfig = {
  apiEndpoints: environment.apiEndpoints,
  loginRedirectUrl: environment.loginRedirectUrl,
  routes: {
    denied: AppRoutes.DENIED,
    auth: AuthRoutes.MODULE_PATH,
    issuer: IssuerRoutes.MODULE_PATH
  }
};

@NgModule({
  providers: [{
    provide: APP_CONFIG,
    useValue: APP_DI_CONFIG
  }]
})
export class AppConfigModule { }
