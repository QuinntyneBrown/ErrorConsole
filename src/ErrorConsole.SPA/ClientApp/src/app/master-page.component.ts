import { Component } from '@angular/core';
import { AuthService } from './core/auth.service';
import { ErrorService } from './core/error.service';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.css'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(private _authService: AuthService, public errorService: ErrorService) {

  }

  public signOut() {
    this._authService.logout();
  }
}
