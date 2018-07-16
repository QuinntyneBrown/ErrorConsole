import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { NotificationService } from "../core/notification.service";
import { tap, takeUntil } from "rxjs/operators";

export abstract class BaseComponent { 
  constructor(protected _notificationService: NotificationService) {
    this.handleError = this.handleError.bind(this);
  }

  public handleError(e: Error) {
    this._notificationService.addError(e);
  }

  private _hasErrors: boolean;

  public get hasErrors(): boolean {
    return this._hasErrors;
  }

  public set hasErrors(value: boolean) {
    if (this._hasErrors && !value) this.recover();

    this._hasErrors = value;
  }
  
  public ngOnInit() {
    this._notificationService.errors$
      .pipe(tap(x => {
        this.hasErrors = x.length > 0;        
      }),takeUntil(this.onDestroy))
      .subscribe();
  }

  public abstract recover();

  public onDestroy: Subject<void> = new Subject<void>();

}
