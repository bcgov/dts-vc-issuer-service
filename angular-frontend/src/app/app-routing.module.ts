import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthRoutes } from '@features/auth/auth.routes';
import { IssuerRoutes } from '@features/issuer/issuer.routes';

const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH, loadChildren: () => import('@features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: IssuerRoutes.MODULE_PATH, loadChildren: () => import('@features/issuer/issuer.module').then(m => m.IssuerModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
