import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SelectionBoxes } from '../../models/SelectionBox';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SelectionBoxStateService {
  baseUrl = environment.baseUrl;
  private selectionSubject: BehaviorSubject<SelectionBoxes> = new BehaviorSubject<SelectionBoxes>(null);
  private errorSelectionsSubject: BehaviorSubject<HttpErrorResponse | null> = new BehaviorSubject<HttpErrorResponse | null>(null);
  selections$ = this.selectionSubject.asObservable();
  errorSelections$ = this.errorSelectionsSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  upsertSelections() {
    this.httpClient.get<SelectionBoxes>(this.baseUrl + "/SelectionBox").pipe(tap((data) => {
      this.selectionSubject.next(data);
      this.errorSelectionsSubject.next(null);
    })).subscribe({
      next: () => {
      }, error: (err) => {
        this.selectionSubject.next(null);
        this.errorSelectionsSubject.next(err);
      }
    });
  }

}