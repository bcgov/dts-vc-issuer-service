import { NgModule } from '@angular/core';
import { TranslocoModule } from '@ngneat/transloco';
import { TranslocoLocaleModule } from '@ngneat/transloco-locale';
import { SharedModule } from '@shared/shared.module';

import { IssuerRoutingModule } from './issuer-routing.module';
import { CredentialsPageComponent } from './pages/credentials-page/credentials-page.component';

@NgModule({
  imports: [
    SharedModule,
    IssuerRoutingModule,
    TranslocoModule,
    TranslocoLocaleModule.init()
  ],
  declarations: [
    CredentialsPageComponent
  ]
})
export class IssuerModule { }
