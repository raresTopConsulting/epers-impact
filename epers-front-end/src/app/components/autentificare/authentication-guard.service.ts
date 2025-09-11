import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AutentificareService } from '../../services/autentificare/autentificare.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuardService  {

  constructor(private authService: AutentificareService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    let isUserAuthenticated = this.authService.isAuthenticated();

    if (isUserAuthenticated) {
      return true;
    } else {
      return this.router.createUrlTree(['/auth/login']);
    }
  }

}
