import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { ProductService } from "./product.service";
import { BaseComponent } from "../core/base.component";
import { Product } from "./product.model";
import { map, takeUntil } from "rxjs/operators";
import { NotificationService } from "../core/notification.service";

@Component({
  templateUrl: "./products-page.component.html",
  styleUrls: ["./products-page.component.css"],
  selector: "app-products-page"
})
export class ProductsPageComponent extends BaseComponent { 
  constructor(private _productService: ProductService,  _notificationService: NotificationService) {
    super(_notificationService)
  }

  public ngOnInit() {
    this._productService
      .get()
      .pipe(map(x => this.products$.next(x)), takeUntil(this.onDestroy))
      .subscribe(null, this.handleError);

    super.ngOnInit();
  }

  public recover() {

  }

  public products$: BehaviorSubject<Product[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
