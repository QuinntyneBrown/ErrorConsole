import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { CompanyService } from "./company.service";
import { Company } from "./company.model";
import { map, takeUntil } from "rxjs/operators";
import { BasePageComponent } from "../core/base-page.component";
import { NotificationService } from "../core/notification.service";

@Component({
  templateUrl: "./companies-page.component.html",
  styleUrls: ["./companies-page.component.css"],
  selector: "app-companies-page"
})
export class CompaniesPageComponent extends BasePageComponent { 
  constructor(private _companyService: CompanyService, _errorService: NotificationService) {
    super(_errorService);
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
