import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';

import { SharedModule } from '@shared/shared.module';

import { IssuerLoginRoutingModule } from './issuer-login-routing.module';
import { IssuerLoginComponent } from './issuer-login.component';


@NgModule({
  declarations: [
    IssuerLoginComponent
  ],
  imports: [
    SharedModule,
    IssuerLoginRoutingModule,
    TranslocoModule
  ]
})
export class IssuerLoginModule { }
