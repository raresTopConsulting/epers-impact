import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { DropdownSelection } from 'src/app/models/common/Dorpdown';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { FirmeDisplayModel } from 'src/app/models/nomenclatoare/FirmeDisplayModel';
import { NFirme } from 'src/app/models/nomenclatoare/NFirme';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FirmeService {
  private firmeSelectionsSubject: BehaviorSubject<DropdownSelection[]> = new BehaviorSubject<DropdownSelection[]>(null);
  private firmaLoggedInUserSubject: BehaviorSubject<DropdownSelection | null> = new BehaviorSubject<DropdownSelection | null>(null);
  private errorFirmeSelectionsSubject: BehaviorSubject<HttpErrorResponse | null> = new BehaviorSubject<HttpErrorResponse | null>(null);
  firmeSelections$ = this.firmeSelectionsSubject.asObservable();
  errorFirmeSelections$ = this.errorFirmeSelectionsSubject.asObservable();
  firmaLoggedInUser$ = this.firmaLoggedInUserSubject.asObservable();
  idFirmaLoggedInUser: number | null;

  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10
  });
  private firmeDisplayModelSubject: BehaviorSubject<FirmeDisplayModel> = new BehaviorSubject<FirmeDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  firmeDisplayModelObs$ = this.firmeDisplayModelSubject.asObservable();

  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) {
    this.idFirmaLoggedInUser = +JSON.parse(localStorage.getItem('user'))?.idFirma;
  }

  public getAll(currentPage: number, itemsPerPage: number, filter: string) {
    this.tableFiltersSubject.next({
      filter: filter,
      filterFirma: null,
      currentPage: currentPage,
      itemsPerPage: itemsPerPage
    });

    this.httpClient.get<FirmeDisplayModel>(this.baseUrl + "/Firme/paginated", {
      params: {
        currentPage: currentPage,
        itemsPerPage: itemsPerPage,
        filter: filter
      }
    }).pipe(tap((data) => {
      this.firmeDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NFirme> {
    return this.httpClient.get<NFirme>(this.baseUrl + "/Firme/" + id);
  }

  public upsertDDFirme() {
    this.httpClient.get<DropdownSelection[]>(this.baseUrl + "/Firme/dropdown").subscribe({
      next: (data) => {
        this.firmeSelectionsSubject.next(data);
        const idFirmaUser = data.find(frm => frm.IdFirma === this.idFirmaLoggedInUser);
        this.firmaLoggedInUserSubject.next(idFirmaUser);
        this.errorFirmeSelectionsSubject.next(null);
      },
      error: (err) => {
        this.firmeSelectionsSubject.next(null);
        this.errorFirmeSelectionsSubject.next(err);
      }
    });
  }

  public update(locatie: NFirme) {
    return this.httpClient.put<NFirme>(this.baseUrl + "/Firme", locatie);
  }

  public add(locatie: NFirme) {
    return this.httpClient.post(this.baseUrl + "/Firme", locatie);
  }

  public delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Firme/" + id);
  }


}
