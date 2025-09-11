import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NSkills } from 'src/app/models/nomenclatoare/NSkills';
import { SkillsDisplayModel } from 'src/app/models/nomenclatoare/SkillsDisplayModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  baseUrl = environment.baseUrl;

  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private skillsDisplayModelSubject: BehaviorSubject<SkillsDisplayModel> = new BehaviorSubject<SkillsDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  skillsDisplayModelObs$ = this.skillsDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

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

    this.httpClient.get<SkillsDisplayModel>(this.baseUrl + "/Skills", { params: params }).pipe(tap((data) => {
      this.skillsDisplayModelSubject.next(data);
    })).subscribe();
  }

  public get(id: number): Observable<NSkills> {
    return this.httpClient.get<NSkills>(this.baseUrl + "/Skills/" + id);
  }

  public update(id: number, skill: NSkills) {
    return this.httpClient.put<NSkills>(this.baseUrl + "/Skills", skill);
  }

  public add(skill: NSkills) {
    return this.httpClient.post(this.baseUrl + "/Skills", skill);
  }

  public delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Skills/" + id);
  }

}
