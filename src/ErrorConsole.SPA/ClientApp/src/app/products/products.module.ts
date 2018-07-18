import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ProductsPageComponent } from './products-page.component';
import { ProductService } from './product.service';
import { AddProductOverlay } from './add-product-overlay';
import { AddProductOverlayComponent } from './add-product-overlay.component';
import { DigitalAssetsModule } from '../digital-assets/digital-assets.module';

const declarations = [
  ProductsPageComponent,
  AddProductOverlayComponent
];

const providers = [
  ProductService,
  AddProductOverlay
];

const entryComponents = [
  AddProductOverlayComponent
];

@NgModule({
  declarations: declarations,
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
export class ProductsModule { }
