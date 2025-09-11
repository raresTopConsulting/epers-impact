import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { DiviziiDisplayModel } from 'src/app/models/nomenclatoare/DivziiDisplayModel';
import { NDivizii } from 'src/app/models/nomenclatoare/NDvizii';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DiviziiService {
  baseUrl = environment.baseUrl;

  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private diviziiDisplayModelSubject: BehaviorSubject<DiviziiDisplayModel> = new BehaviorSubject<DiviziiDisplayModel>(null);

  tableFiltersDiviziiObs$ = this.tableFiltersSubject.asObservable();
  diviziiDisplayModelObs$ = this.diviziiDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }
  // private angularHttpUtils: AngularHttpUtils) { }

  public getAllPaginated(currentPage, itemsPerPage, filter, filterFirma: number | null) {
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

    this.httpClient.get<DiviziiDisplayModel>(this.baseUrl + "/Divizii", { params: params }).pipe(tap((data) => {
      this.diviziiDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NDivizii> {
    return this.httpClient.get<NDivizii>(this.baseUrl + "/Divizii/" + id);
  }

  public update(divizie: NDivizii) {
    return this.httpClient.put<NDivizii>(this.baseUrl + "/Divizii", divizie);
  }

  public add(divizie: NDivizii) {
    return this.httpClient.post(this.baseUrl + "/Divizii", divizie);
  }

  public delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Divizii/" + id);
  }

}
