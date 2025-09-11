import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { CompartimenteDisplayModel } from 'src/app/models/nomenclatoare/CompartimenteDisplayModel';
import { NCompartimentDisplay, NCompartimente } from 'src/app/models/nomenclatoare/NCompartimente';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompartimenteService {

constructor(private httpClient: HttpClient) {}
  baseUrl = environment.baseUrl;
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private compartimenteDisplayModelSubject: BehaviorSubject<CompartimenteDisplayModel> = new BehaviorSubject<CompartimenteDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  compartimenteDisplayModelObs$ = this.compartimenteDisplayModelSubject.asObservable();
  
  public getCompartimente(): Observable<string[]> {
    return this.httpClient.get<string[]>(this.baseUrl + "/Compartimente");
  }

  public getAllPaginated(currentPage: number, itemsPerPage: number, filter: string, filterFirma: number | null) {
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

    this.httpClient.get<CompartimenteDisplayModel>(this.baseUrl + "/Compartimente", { params: params} ).pipe(tap((data) => {
      this.compartimenteDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NCompartimentDisplay> {
    return this.httpClient.get<NCompartimentDisplay>(this.baseUrl + "/Compartimente/" + id);
  }

  public update(compartiment: NCompartimente) {
    return this.httpClient.put<NCompartimente>(this.baseUrl + "/Compartimente/", compartiment);
  }

  public add(compartiment: NCompartimente) {
    return this.httpClient.post(this.baseUrl + "/Compartimente", compartiment);
  }

  public delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Compartimente/" + id);
  }

}
