import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { NotificationService } from "../core/notification.service";

export class BaseComponent { 
  constructor(protected _notificationService: NotificationService) {
    this.handleError = this.handleError.bind(this);
  }

  public handleError(e: Error) {
    this._notificationService.addError(e);
  }
}
