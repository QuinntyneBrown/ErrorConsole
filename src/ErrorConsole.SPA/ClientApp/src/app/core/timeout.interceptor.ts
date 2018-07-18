import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpSentEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { accessTokenKey } from '../core/constants';
import { LocalStorageService } from '../core/local-storage.service';
import { RedirectService } from './redirect.service';
import { NotificationService } from './notification.service';


@Injectable()
export class TimeoutInterceptor implements HttpInterceptor {
  constructor() { }
  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      tap(
        (httpEvent: HttpEvent<any>) => {
          httpEvent.type == HttpEventType.Sent;

          console.log(`${httpEvent.type}: ${Date.now()}`);
          
          //console.log(JSON.stringify(httpEvent));
        }
      )
    );
  }
}
