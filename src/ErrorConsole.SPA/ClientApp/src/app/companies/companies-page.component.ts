import { Component } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";
import { map, takeUntil } from "rxjs/operators";
import { BasePageComponent } from "../core/base-page.component";
import { NotificationService } from "../core/notification.service";
import { Company } from "./company.model";
import { CompanyService } from "./company.service";

@Component({
  templateUrl: "./companies-page.component.html",
  styleUrls: ["./companies-page.component.css"],
  selector: "app-companies-page"
})
export class CompaniesPageComponent extends BasePageComponent { 
  constructor(private _companyService: CompanyService, _notificationService: NotificationService) {
    super(_notificationService);
  }
  
  ngOnInit() {
    this._companyService
      .get()
      .pipe(map(x => this.companies$.next(x)),takeUntil(this.onDestroy))
      .subscribe(null, this.handleError);
  }
  public companies$: BehaviorSubject<Company[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public refresh() {
    this.companies$.next([]);
    this.ngOnInit();
  }
}
