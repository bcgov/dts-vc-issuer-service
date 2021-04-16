import { NgModule } from '@angular/core';

import { CoreModule } from '@core/core.module';

import { AppComponent } from './app.component';
import { AppConfigModule } from './app-config.module';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { TranslocoRootModule } from './transloco/transloco-root.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    CoreModule,
    AppConfigModule,
    AppRoutingModule,
    HttpClientModule,
    TranslocoRootModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
