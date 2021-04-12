import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { IssuerRoutingModule } from './issuer-routing.module';
import { CredentialsPageComponent } from './pages/credentials-page/credentials-page.component';
import { IssuancePageComponent } from './pages/issuance-page/issuance-page.component';
import { MatCardExpansionComponent } from './shared/components/mat-card-expansion/mat-card-expansion.component';
import { PageHeaderComponent } from './shared/components/page-header/page-header.component';
import { PageHeaderSummaryDirective } from './shared/components/page-header/page-header-summary.directive';

@NgModule({
  imports: [
    SharedModule,
    IssuerRoutingModule
  ],
  declarations: [
    CredentialsPageComponent,
    IssuancePageComponent,
    MatCardExpansionComponent,
    PageHeaderComponent,
    PageHeaderSummaryDirective
  ]
})
export class IssuerModule { }
