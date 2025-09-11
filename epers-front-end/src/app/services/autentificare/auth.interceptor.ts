import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError } from "rxjs";
import { AutentificareService } from "./autentificare.service";
import { Router } from "@angular/router";
import { Injectable } from "@angular/core";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject(null);

    constructor(private authService: AutentificareService,
        private router: Router
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const clonedRequest = req.clone({ withCredentials: true });

        return next.handle(clonedRequest).pipe(
            catchError((error: HttpErrorResponse) => {
                // Check if token expired (401 Unauthorized)
                if (error.status === 401 && !clonedRequest.url.includes("/Authentication/login")) {
                    return this.handle401Error(clonedRequest, next);
                }
                return throwError(() => error);
            })
        );
    }

    handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authService.refreshToken().pipe(
                switchMap((resp: any) => {
                    this.isRefreshing  = false;
                    this.refreshTokenSubject.next(true);
                    return next.handle(request);
                }),
                catchError((err) => {
                    this.isRefreshing = false;
                    this.refreshTokenSubject.next(null);
                    this.authService.logout();
                    return throwError(() => err);
                })
            );
        } else {
            return this.refreshTokenSubject.pipe(
                filter(token => token != null),
                take(1),
                switchMap(() => next.handle(request))
            );
        }
    }
}
