import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CompanyService } from "./company.service";
import { Company } from "./company.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./add-company.component.html",
  styleUrls: ["./add-company.component.css"],
  selector: "app-add-company",
  host: { 'class':'mat-typography' }
})
export class AddCompanyComponent { 
  constructor(
    private _companyService: CompanyService,
    private _overlay: OverlayRefWrapper) {
    this.handleError = this.handleError.bind(this);
  }

  ngOnInit() {
    if (this.companyId)
      this._companyService.getById({ companyId: this.companyId })
        .pipe(
          map(x => this.company$.next(x)),
          switchMap(x => this.company$),
          map(x => this.form.patchValue({
            name: x.name
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public company$: BehaviorSubject<Company> = new BehaviorSubject(<Company>{});
  
  public companyId: number;

  public handleCancelClick() {
    this._overlay.close()
  }

  public handleSaveClick() {
    const company = new Company();
    company.companyId = this.companyId;
    company.name = this.form.value.name;
    this._companyService.create({ company })
      .pipe(
        map(x => company.companyId = x.companyId),
        tap(x => this._overlay.close(company)),
        takeUntil(this.onDestroy)
      )
      .subscribe(null,this.handleError);
  }

  public errorMessage: string;
  
  public handleError(e) {
    this.errorMessage = e.message; 
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
