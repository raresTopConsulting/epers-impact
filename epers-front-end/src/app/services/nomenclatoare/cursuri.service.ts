import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { CursuriDisplayModel } from 'src/app/models/nomenclatoare/CursuriDisplayModel';
import { NCursuri } from 'src/app/models/nomenclatoare/NCursuri';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CursuriService {
  baseUrl = environment.baseUrl;

  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private cursuriDisplayModelSubject: BehaviorSubject<CursuriDisplayModel> = new BehaviorSubject<CursuriDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  cursuriDisplayModelObs$ = this.cursuriDisplayModelSubject.asObservable();
  
  constructor(private httpClient: HttpClient) { }

  public getCursuri(): Observable<string[]> {
    return this.httpClient.get<string[]>(this.baseUrl + "/Cursuri");
  }

  public getAll(currentPage: number, itemsPerPage: number, filter: string, filterFirma: number | null) {
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

    this.httpClient.get<CursuriDisplayModel>(this.baseUrl + "/Cursuri", { params: params }).pipe(tap((data) => {
      this.cursuriDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NCursuri> {
    return this.httpClient.get<NCursuri>(this.baseUrl + "/Cursuri/" + id);
  }

  public update(curs: NCursuri) {
    return this.httpClient.put<NCursuri>(this.baseUrl + "/Cursuri", curs);
  }

  public add(curs: NCursuri) {
    return this.httpClient.post(this.baseUrl + "/Cursuri", curs);
  }

  public delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Cursuri/" + id);
  }

}
