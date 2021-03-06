import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';
import { NotificationService } from './notification.service';
import { HeaderInterceptor } from './headers.interceptor';
import { LocalStorageService } from './local-storage.service';
import { Logger } from './logger.service';
import { RedirectService } from './redirect.service';
import { HttpErrorResponseInterceptor } from './http-error-response.interceptor';
import { OverlayRefProvider } from './overlay-ref-provider';
import { OverlayRefWrapper } from './overlay-ref-wrapper';
import { TimeoutInterceptor } from './timeout.interceptor';

const providers = [
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HeaderInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorResponseInterceptor,
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: TimeoutInterceptor,
    multi: true
  },
  AuthGuard,
  AuthService,
  NotificationService,
  OverlayRefProvider,
  OverlayRefWrapper,
  LocalStorageService,
  RedirectService,
  Logger
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule
  ],
  providers,
  exports: []
})
export class CoreModule {}
