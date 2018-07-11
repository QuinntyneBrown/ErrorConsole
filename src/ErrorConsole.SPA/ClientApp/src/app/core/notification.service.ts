import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable()
export class NotificationService {
  constructor(private _snackBar: MatSnackBar) {}

  public errors$: BehaviorSubject<any[]> = new BehaviorSubject([]);

  public addError(e) {    
    this.errors$.next([e, ...this.errors$.value]);
  }

  public handle(
    httpErrorResponse: HttpErrorResponse,
    message: string = 'Error',
    action: string = 'An error ocurr.Try it again.'
  ): MatSnackBarRef<SimpleSnackBar> {
    return this._snackBar.open(message, action, {
      duration: 0
    });
  }
}
