import { Component, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subscription } from 'rxjs';
import { AutentificareService } from 'src/app/services/autentificare/autentificare.service';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { environment } from 'src/environments/environment';

@UntilDestroy({ checkProperties: true })
@Component({
    selector: 'navbar',
    templateUrl: './navbar.component.html',
    standalone: false
})
export class NavbarComponent implements OnInit {
  isUserAuth: boolean;
  userRoleId: number;
  isUserAdmin: boolean;
  userId: number;
  companyName: string;
  userQuerySub: Subscription;
  userAuthenticatedSub: Subscription;

  constructor(private authService: AutentificareService,
    private userStateService: UserStateService) { }

  ngOnInit() {
    this.companyName = environment.companyName;

    this.userQuerySub = this.userStateService.loggedInUser$.subscribe((data) => {
      if (data) {
        this.userId = data.id;
        this.userRoleId = data.idRol;
        this.isUserAdmin = data.idRol === 1;
      } else {
        this.isUserAuth = this.authService.isAuthenticated();
        if (this.isUserAuth) {
          this.userStateService.upsertUserDataWithLocalStorage();
          this.userRoleId = this.authService.getUserRoleId();
          this.userId = localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user"))?.id : null;
          this.isUserAdmin = this.userRoleId === 1;
        } else {
          this.userId = null;
          this.userRoleId = null;
        }
      }
    });

    this.userAuthenticatedSub = this.authService.isAuthenticated$().subscribe((isAuth) => {
      this.isUserAuth = isAuth;
    });
  }
}
