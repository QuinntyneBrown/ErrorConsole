import { Component, HostBinding } from '@angular/core';
import { AuthService } from './core/auth.service';
import { NotificationService } from './core/notification.service';
import { map, tap, takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.css'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(private _authService: AuthService, public notificationService: NotificationService) { }

  ngOnInit() {
    this.notificationService.errors$
      .pipe(tap(x => {
        if (this.isErrorConsoleOpen === null) {
          this.isErrorConsoleOpen = false;
        }
        else {
          this.isErrorConsoleOpen = true;
        }
      }))
      .subscribe();
  }
  
  public signOut() {
    this._authService.logout();
  }

  public closeErrorConsole() {
    this.isErrorConsoleOpen = false;
  }

  @HostBinding("class.error-console-is-opened")
  public isErrorConsoleOpen:boolean = null;
}
