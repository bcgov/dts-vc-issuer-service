import { NgModule } from '@angular/core';

import { CoreModule } from '@core/core.module';

import { AppComponent } from './app.component';
import { AppConfigModule } from './app-config.module';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    CoreModule,
    AppConfigModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
