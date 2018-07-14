import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CompanyService } from './company.service';
import { CompaniesPageComponent } from './companies-page.component';
import { AddCompanyComponent } from './add-company.component';
import { AddCompany } from './add-company';

const declarations = [
  CompaniesPageComponent,
  AddCompanyComponent
];

const providers = [
  CompanyService,
  AddCompany
];

const entryComponents = [
  AddCompanyComponent
]

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class CompaniesModule { }
