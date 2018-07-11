import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ErrorService } from "../core/error.service";

export class BasePageComponent { 
  constructor(protected _errorService: ErrorService) {
    this.handleError = this.handleError.bind(this);
  }

  public handleError(e) {
    this._errorService.collect(e);
  }
}
