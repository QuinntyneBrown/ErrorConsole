import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { NotificationService } from "../core/notification.service";

export class BasePageComponent { 
  constructor(protected _errorService: NotificationService) {
    this.handleError = this.handleError.bind(this);
  }

  public handleError(e) {
    this._errorService.collect(e);
  }
}
