import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { Concluzie } from 'src/app/models/concluzii/Concluzie';
import { ConcluziiListaSubalterniDisplayModel } from 'src/app/models/concluzii/ConcluziiListaSubalterni';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConcluziiService {
  baseUrl = environment.baseUrl;
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10
  });
  private concluziiDisplayModelSubject: BehaviorSubject<ConcluziiListaSubalterniDisplayModel> = new BehaviorSubject<ConcluziiListaSubalterniDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  concluziiDisplayModelObs$ = this.concluziiDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getListaSubalterni(currentPage, itemsPerPage, filter, filterFirma) {
    const matricolaLoggedInUser = JSON.parse(localStorage.getItem('user')).matricola;
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
    params = params.append("matricolaLoggedInUser", matricolaLoggedInUser ? matricolaLoggedInUser : '');

    this.tableFiltersSubject.next({
      filter: filter,
      filterFirma: filterFirma,
      currentPage: currentPage,
      itemsPerPage: itemsPerPage 
    });

    return this.httpClient.get<ConcluziiListaSubalterniDisplayModel>(this.baseUrl + "/Concluzii/listaSubalterni", { params: params }).pipe(tap((data)=> {
      this.concluziiDisplayModelSubject.next(data);
    })).subscribe();
  }

  addConcluzie(concluzie: Concluzie, idEvaluari: number[]) {
    let loggedInUserMatricola = JSON.parse(localStorage.getItem('user')).matricola;
    return this.httpClient.post(this.baseUrl + "/Concluzii/add", concluzie, {
      params: {
        idEvaluari: idEvaluari,
        loggedInUserMatricola: loggedInUserMatricola
      }
    });
  }

  getIdsEvaluariSubaltern(idAngajat: number, year: number | null): Observable<number[]> {
    return this.httpClient.get<number[]>(this.baseUrl + "/Concluzii/idsEvaluariSubaltern", {
      params: {
        idAngajat: idAngajat,
        anul: year ? year : ""
      }
    });
  }

  getIstoric(idAngajat: number, anul: number | null): Observable<Concluzie> {
    return this.httpClient.get<Concluzie>(this.baseUrl + "/Concluzii/istoric", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

}
