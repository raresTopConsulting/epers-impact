import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { AfisareNObiectiveDisplayModel, NObiective } from 'src/app/models/nomenclatoare/NObiective';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NobiectiveService {

  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;

  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10
  });
  private afisareNObDisplayModelSubject: BehaviorSubject<AfisareNObiectiveDisplayModel> = new BehaviorSubject<AfisareNObiectiveDisplayModel>(null);
  tableFiltersObiect$ = this.tableFiltersSubject.asObservable();
  afisareNObDisplayModelObs$ = this.afisareNObDisplayModelSubject.asObservable()

  public getAllPaginated(currentPage, itemsPerPage, filter, filterFirma) {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;

    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage);
    params = params.append("itemsPerPage", itemsPerPage);
    params = params.append("idRol", idRolLoggedInUser ? idRolLoggedInUser : '');
    params = params.append("idFirmaLoggedInUser", idFirmaLoggedInUser ? idFirmaLoggedInUser : '');
    params = params.append("filter", filter ? filter : '');
    params = params.append("filterFirma", filterFirma ? filterFirma : '');

    this.tableFiltersSubject.next({
      filter: filter,
      filterFirma: filterFirma,
      currentPage: currentPage,
      itemsPerPage: itemsPerPage
    });

    this.httpClient.get<AfisareNObiectiveDisplayModel>(this.baseUrl + "/NObiective", { params: params }).pipe(tap((data) => {
      this.afisareNObDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NObiective> {
    return this.httpClient.get<NObiective>(this.baseUrl + "/NObiective/" + id);
  }

  public update(nObiectiv: NObiective) {
    return this.httpClient.put<NObiective>(this.baseUrl + "/NObiective", nObiectiv);
  }

  public add(nObiectiv: NObiective) {
    return this.httpClient.post(this.baseUrl + "/NObiective", nObiectiv);
  }

}
