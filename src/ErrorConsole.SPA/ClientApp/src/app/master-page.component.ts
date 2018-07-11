import { Component } from '@angular/core';
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
      .pipe(tap(x => { }))
      .subscribe();
  }

  public toggleErrorConsole() {

  }

  public signOut() {
    this._authService.logout();
  }


}
