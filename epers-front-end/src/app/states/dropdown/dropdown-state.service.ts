import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppDropdownSelections } from 'src/app/models/common/Dorpdown';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class DropdownStateService {
    baseUrl = environment.baseUrl;
    private appDropdownsSubject: BehaviorSubject<AppDropdownSelections> = new BehaviorSubject<AppDropdownSelections>(null);
    private errorSelectionsSubject: BehaviorSubject<HttpErrorResponse | null> = new BehaviorSubject<HttpErrorResponse | null>(null);
    appDropdownsSubject$ = this.appDropdownsSubject.asObservable();
    errorSelections$ = this.errorSelectionsSubject.asObservable();

    constructor(private httpClient: HttpClient) { }

    upsertDropdowns() {
        this.httpClient.get<AppDropdownSelections>(this.baseUrl + "/Dropdown", {withCredentials: true}).pipe(tap((data: AppDropdownSelections) => {
            this.appDropdownsSubject.next(data);
            this.errorSelectionsSubject.next(null);
        })).subscribe({
            next: () => {
            }, error: (err) => {
                this.appDropdownsSubject.next(null);
                this.errorSelectionsSubject.next(err);
            }
        });
    }
}
