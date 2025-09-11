import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NObiective } from 'src/app/models/nomenclatoare/NObiective';
import { Obiective, ObiectiveDisplayModel as ObiectiveDisplayModel, ObiectiveListaSubalterniDisplayModel } from 'src/app/models/obiective/Obiective';
import { SetareObiective } from 'src/app/models/obiective/SetareOb';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ObiectiveService {
  baseUrl = environment.baseUrl;
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private obiectiveDisplayModelSubject: BehaviorSubject<ObiectiveListaSubalterniDisplayModel> = new BehaviorSubject<ObiectiveListaSubalterniDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  obiectiveDisplayModelObs$ = this.obiectiveDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  public getListaSubalterni(currentPage, itemsPerPage, filter, filterFirma: number | null) {
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

    this.httpClient.get<ObiectiveListaSubalterniDisplayModel>(this.baseUrl + "/Obiective/listaSubalterni", {params: params}).pipe(tap((data) => {
      this.obiectiveDisplayModelSubject.next(data);
    })).subscribe();
  }

  getObFromNomOb(id: number): Observable<NObiective> {
    return this.httpClient.get<NObiective>(this.baseUrl + "/Obiective/nomObiectiv/" + id);
  }

  setareObiectiv(setareObiective: SetareObiective, idAngajatiSelectati: number[] | null) {
    return this.httpClient.post(this.baseUrl + "/Obiective/setareObiective", setareObiective, {
      params: {
        idAngajatiSelectati: idAngajatiSelectati
      }
    });
  }

  getObiectiveActuale(idAngajat: number, currentPage: number, itemsPerPage: number, filter:string | null): Observable<ObiectiveDisplayModel> {
    return this.httpClient.get<ObiectiveDisplayModel>(this.baseUrl + "/Obiective/actuale", {
      params: {
        idAngajat: idAngajat,
        currentPage: currentPage,
        itemsPerPage: itemsPerPage,
        filter: filter
      }
    });
  }

  getIstoricObiective(idAngajat: number, currentPage: number, itemsPerPage: number, filter:string | null): Observable<ObiectiveDisplayModel> {
    return this.httpClient.get<ObiectiveDisplayModel>(this.baseUrl + "/Obiective/istoric", {
      params: {
        idAngajat: idAngajat,
        currentPage: currentPage,
        itemsPerPage: itemsPerPage,
        filter: filter
      }
    });
  }

  evaluareObiective(obiective: Obiective[]) {
    return this.httpClient.put<Obiective[]>(this.baseUrl + "/Obiective/evaluareObiective", obiective);
  }

}
