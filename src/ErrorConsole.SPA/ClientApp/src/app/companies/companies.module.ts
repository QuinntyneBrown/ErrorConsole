import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CompanyService } from './company.service';
import { CompaniesPageComponent } from './companies-page.component';

import { DigitalAssetsModule } from '../digital-assets/digital-assets.module';
import { AddCompanyOverlayComponent } from './add-company-overlay.component';
import { AddCompanyOverlay } from './add-company-overlay';

const declarations = [
  CompaniesPageComponent,
  AddCompanyOverlayComponent
];

const providers = [
  CompanyService,
  AddCompanyOverlay
];

const entryComponents = [
  AddCompanyOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    DigitalAssetsModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class CompaniesModule { }
