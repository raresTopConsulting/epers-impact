import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { AfisareEvalCalificativFinal, AfisareSkillsEvalModel } from 'src/app/models/evaluare/AfisareEval';
import { EvaluareListaSubalterniDisplayModel } from 'src/app/models/evaluare/EvalListaSubalterni';
import { EvaluareTemplate } from 'src/app/models/evaluare/Evaluare';
import { Notite } from 'src/app/models/evaluare/Mentiuni';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EvaluareService {
  baseUrl = environment.baseUrl;
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10
  });
  private evaluareDisplayModelSubject: BehaviorSubject<EvaluareListaSubalterniDisplayModel> = new BehaviorSubject<EvaluareListaSubalterniDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  evaluareDisplayModelObs$ = this.evaluareDisplayModelSubject.asObservable();
  
  constructor(private httpClient: HttpClient) { }

  public getListaSubalterni(currentPage, itemsPerPage, filter, filterFirma) {
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

    return this.httpClient.get<EvaluareListaSubalterniDisplayModel>(this.baseUrl + "/Evaluare/listaSubalterni", { params: params }).pipe(tap((data) => {
      this.evaluareDisplayModelSubject.next(data);
    })).subscribe();
  }

  public getAfisareSkillsEval(idAngajat: number, anul: number | null): Observable<AfisareSkillsEvalModel> {
    return this.httpClient.get<AfisareSkillsEvalModel>(this.baseUrl + "/Evaluare/afisareSkillsEval", {
      params: {
        idAngajat: idAngajat,
        anul: anul
      }
    });
  }

  public getIstoricEval(idAngajat: number, anul: number | null): Observable<AfisareSkillsEvalModel> {
    return this.httpClient.get<AfisareSkillsEvalModel>(this.baseUrl + "/Evaluare/istoric", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  getIstoricEvalCalificativFinal(idAngajat: number, anul: number | null): Observable<AfisareEvalCalificativFinal> {
    return this.httpClient.get<AfisareEvalCalificativFinal>(this.baseUrl + "/Evaluare/istoric/calificativFinal", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    })
  }

  public upsertEvaluare(evaluare: EvaluareTemplate) {
    return this.httpClient.put<EvaluareTemplate>(this.baseUrl + "/Evaluare/upsertEvaluare", evaluare);
  }

  public addMentiune(mentiuni: Notite) {
    return this.httpClient.post(this.baseUrl + "/Evaluare/mentiuni", mentiuni);
  }

  public getMentiuni(idAngajat: number, anul: number): Observable<Notite[]> {
    return this.httpClient.get<Notite[]>(this.baseUrl + "/Evaluare/mentiuni", {
      params: {
        idAngajat: idAngajat,
        anul: anul
      }
    });
  }

  public contestareEvaluare(idAngajat: number, anul: number) {
    return this.httpClient.get(this.baseUrl + "/Evaluare/contestareEvaluare", {
      params: {
        idAngajat: idAngajat,
        anul: anul
      }
    });
  }

}
