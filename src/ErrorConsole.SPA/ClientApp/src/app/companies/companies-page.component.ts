import { Component } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";
import { map, takeUntil, tap } from "rxjs/operators";
import { BaseComponent } from "../core/base.component";
import { NotificationService } from "../core/notification.service";
import { Company } from "./company.model";
import { CompanyService } from "./company.service";
import { AddCompany } from "./add-company";

@Component({
  templateUrl: "./companies-page.component.html",
  styleUrls: ["./companies-page.component.css"],
  selector: "app-companies-page"
})
export class CompaniesPageComponent extends BaseComponent { 
  constructor(
    private _addCompany: AddCompany,
    private _companyService: CompanyService,
    _notificationService: NotificationService) {
    super(_notificationService);
  }
  
  public ngOnInit() {
    this._companyService
      .get()
      .pipe(map(x => this.companies$.next(x)),takeUntil(this.onDestroy))
      .subscribe(null, this.handleError);

    super.ngOnInit();
  }

  public recover() {
    this._companyService
      .get()
      .pipe(map(x => this.companies$.next(x)), takeUntil(this.onDestroy))
      .subscribe(null, this.handleError);
  }

  public companies$: BehaviorSubject<Company[]> = new BehaviorSubject([]);
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public refresh() {
    this.companies$.next([]);
    this.ngOnInit();
  }

  public openAddCompanyOverlay() {
    this._addCompany
      .create()
      .pipe(tap(x => {
        if(x) this.companies$.next([...this.companies$.value, x]);
      }))
      .subscribe();
  }
}
