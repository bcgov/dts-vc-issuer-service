import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { IssuerRoutingModule } from './issuer-routing.module';
import { CredentialsPageComponent } from './pages/credentials-page/credentials-page.component';
import { IssuancePageComponent } from './pages/issuance-page/issuance-page.component';

@NgModule({
  imports: [
    SharedModule,
    IssuerRoutingModule
  ],
  declarations: [
    CredentialsPageComponent,
    IssuancePageComponent
  ]
})
export class IssuerModule { }
