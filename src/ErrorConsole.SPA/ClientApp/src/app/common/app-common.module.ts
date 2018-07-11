import { CommonModule } from '@angular/common';

import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';

const declarations = [
  
];

const providers = [

];

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    SharedModule
  ],
  providers,
  declarations,
  exports: []
})
export class AppCommonModule { }
