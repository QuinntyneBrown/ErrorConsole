import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { accessTokenKey } from '../core/constants';
import { LocalStorageService } from '../core/local-storage.service';
import { RedirectService } from './redirect.service';
import { NotificationService } from './notification.service';

@Injectable()
export class HttpErrorResponseInterceptor implements HttpInterceptor {
  constructor(
    private _localStorageService: LocalStorageService,
    private _redirectService: RedirectService,
    private _errorService: NotificationService
  ) {}
  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      tap(
        (httpEvent: HttpEvent<any>) => httpEvent,
        error => {
          if (error instanceof HttpErrorResponse && error.status === 401) {
            this._localStorageService.put({ name: accessTokenKey, value: null });
            this._redirectService.redirectToLogin();
          }

          //if (error instanceof HttpErrorResponse && error.status != 401) {
          //  this._errorService.handle(error);
          //}
        }
      )
    );
  }
}
