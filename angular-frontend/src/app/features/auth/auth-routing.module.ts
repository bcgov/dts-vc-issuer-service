import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IssuerRoutes } from '../issuer/issuer.routes';

import { AuthRoutes } from './auth.routes';
import { AuthorizationRedirectGuard } from './shared/guards/authorization-redirect.guard';

const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH,
    children: [
      {
        path: IssuerRoutes.LOGIN_PAGE,
        canLoad: [AuthorizationRedirectGuard],
        loadChildren: () => import('@features/issuer/shared/modules/issuer-login/issuer-login.module').then(m => m.IssuerLoginModule)
      },
      {
        path: '', // Equivalent to `/` and alias for `info`
        redirectTo: IssuerRoutes.LOGIN_PAGE,
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
