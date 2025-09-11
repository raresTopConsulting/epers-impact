import { Component, OnInit } from '@angular/core';
import { AutentificareService } from 'src/app/services/autentificare/autentificare.service';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserStateService } from 'src/app/services/user/user-state.service';
import { UntilDestroy } from '@ngneat/until-destroy';

@UntilDestroy({checkProperties: true})
@Component({
    selector: 'user-profile',
    templateUrl: './user-profile.component.html',
    standalone: false
})
export class UserProfileComponent implements OnInit {
  isUserAuth: boolean;
  userIsAdminOrHR: boolean = false;
  userId: number;
  companyName: string;
  displayUserSetup: boolean;
  displayGenerarePdfExcelEvaluari: boolean;
  userQuerySub: Subscription;
  userAuthenticatedSub: Subscription;

  constructor(private authService: AutentificareService,
    private userStateService: UserStateService) { }

  ngOnInit() {
    this.companyName = environment.companyName;
    this.displayUserSetup = environment.displayUserSetup;
    this.displayGenerarePdfExcelEvaluari = environment.displayGenerarePdfExcelEvaluari;
    const loggedInUserIdRol = JSON.parse(localStorage.getItem('user'))?.idRol;
    this.userIsAdminOrHR = loggedInUserIdRol === 1 || loggedInUserIdRol === 4 ? true: false;

    this.userQuerySub = this.userStateService.loggedInUser$.subscribe((data) => {
      if (data) {
        this.userId = data.id;
      } else {
        this.isUserAuth = this.authService.isAuthenticated(); 
        if (this.isUserAuth) {
          this.userStateService.upsertUserDataWithLocalStorage();
          this.userId = localStorage.getItem("user") ? JSON.parse(localStorage.getItem("user"))?.id : null;
        }
      }
    });

    this.userAuthenticatedSub = this.authService.isAuthenticated$().subscribe((isAuth) => {
      this.isUserAuth = isAuth;
    });
  }

  onLogout() {
    this.isUserAuth = false;
  }
}
