import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, mergeMap, Observable, of, tap, throwError } from 'rxjs';
import { LoggedInUserData, UserAuthenticationRequest } from 'src/app/models/useri/User';
import { environment } from 'src/environments/environment';
import { UserStateService } from '../user/user-state.service';
import { SelectionBoxStateService } from 'src/app/states/selectionBox/selectionBox.service';
import { DropdownStateService } from 'src/app/states/dropdown/dropdown-state.service';
import { FirmeService } from '../nomenclatoare/firme.service';
import { ErrorResponse } from 'src/app/models/error/ErrorResponseModel';

@Injectable({
  providedIn: 'root'
})
export class AutentificareService {
  baseUrl = environment.baseUrl;
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  refreshTimeout: any;

  constructor(private httpClient: HttpClient,
    private userStateService: UserStateService,
    private ddStateService: DropdownStateService,
    private firmeService: FirmeService,
    private selectionBoxStateService: SelectionBoxStateService,
    private router: Router) { }

  login(user: UserAuthenticationRequest) {
    return this.httpClient.post<LoggedInUserData | ErrorResponse>(this.baseUrl + "/Authentication/login", user, { withCredentials: true }).pipe(
      tap((userResp) => {
        if (userResp && typeof userResp === 'object' && 'id' in userResp) {
          this.handleAuthentication(userResp);
          this.scheduleTokenRefresh(userResp.expiresIn);
          this.ddStateService.upsertDropdowns();
          this.selectionBoxStateService.upsertSelections();
          this.firmeService.upsertDDFirme();
        }
      }),
      catchError((err: HttpErrorResponse) => {
        return throwError(() => err); // forward error to subscriber
      }));
  }

  refreshToken() {
    return this.httpClient.post<number>(this.baseUrl + "/Authentication/refresh-token", {})
      .pipe(tap((expIn: number) => {
        this.scheduleTokenRefresh(expIn);
      }));
  }

  isAuthenticated$(): Observable<boolean> {
    this.checkAuthStatus();
    return this.isAuthenticatedSubject.asObservable();
  }

  isAuthenticated(): boolean {
    this.checkAuthStatus();
    return this.isAuthenticatedSubject.value;
  }

  logout() {
    this.httpClient.post(this.baseUrl + "/Authentication/logout/", null).subscribe(() => {
      this.userStateService.loggedInUserSubject.next(null);
      this.isAuthenticatedSubject.next(false);
      localStorage.removeItem("user");
      this.router.navigate(['']);
    });
  }

  logoutUserNoNavigation() {
    this.httpClient.post(this.baseUrl + "/Authentication/logout/", null).subscribe(() => {
      this.userStateService.loggedInUserSubject.next(null);
      localStorage.removeItem("user");
    });
  }

  getUserRoleId() {
    if (this.isAuthenticated()) {
      return JSON.parse(localStorage.getItem('user')).idRol;
    }
  }

  private checkAuthStatus() {
    const loggedInUserId = JSON.parse(localStorage.getItem('user'))?.id;
    if (loggedInUserId && this.isAuthenticatedSubject.getValue() === false) {
      this.isAuthenticatedSubject.next(true);
    }
  }

  private handleAuthentication(userResp: LoggedInUserData) {
    const loggedInUser = {
      id: userResp.id,
      idRol: userResp.idRol,
      idPost: userResp.idPost,
      idLocatie: userResp.idLocatie,
      idCompartiment: userResp.idCompartiment,
      idSuperior: userResp.idSuperior,
      idFirma: userResp.idFirma,
      matricola: userResp.matricola,
      rol: userResp.rol,
      numePrenume: userResp.numePrenume,
      username: userResp.username,
      matricolaSuperior: userResp.matricolaSuperior,
      numeSuperior: userResp.numeSuperior,
      expiresIn: userResp.expiresIn
    };
    this.userStateService.loggedInUserSubject.next(loggedInUser);
    localStorage.setItem('user', JSON.stringify(loggedInUser));
    this.isAuthenticatedSubject.next(true);
  }

  private scheduleTokenRefresh(expiresIn: number) {
    if (this.refreshTimeout) {
      clearTimeout(this.refreshTimeout);
    }
      // Refresh 5 minutes before expiration
    const refreshTime = (expiresIn * 1000) - (5 * 60 * 1000);

    this.refreshTimeout = setTimeout(() => {
      this.refreshToken().subscribe();
    }, refreshTime);
  }

}
