import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnonymousMasterPageComponent } from './anonymous-master-page.component';
import { CompaniesPageComponent } from './companies/companies-page.component';
import { AuthGuard } from './core/auth.guard';
import { MasterPageComponent } from './master-page.component';
import { ProductsPageComponent } from './products/products-page.component';
import { LoginComponent } from './users/login.component';

export const routes: Routes = [
  {
    path: 'login',
    component: AnonymousMasterPageComponent,
    children: [
      {
        path: '',
        component: LoginComponent
      }
    ]
  },
  {
    path: '',
    component: MasterPageComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: CompaniesPageComponent,
      },
      {
        path: 'products',
        component: ProductsPageComponent,
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
