import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AutentificareService } from 'src/app/services/autentificare/autentificare.service';

@Component({
    selector: 'logout',
    templateUrl: './logout.component.html',
    standalone: false
})
export class LogoutComponent {
  @Input() isUserAuth: boolean;

  constructor(private authService: AutentificareService) { }
  
  logout() {
    this.authService.logout();
  }

}
