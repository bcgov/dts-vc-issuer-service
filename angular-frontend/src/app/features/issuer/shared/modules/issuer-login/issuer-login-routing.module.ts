import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssuerLoginComponent } from './issuer-login.component';

const routes: Routes = [
  {
    path: '',
    component: IssuerLoginComponent,
    data: { title: 'Issuer' }
  }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IssuerLoginRoutingModule { }
