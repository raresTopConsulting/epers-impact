import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NStariPIP } from 'src/app/models/nomenclatoare/NStariPIP';
import { ListaSubalterniCalificatiPipDisplayModel } from 'src/app/models/pip/ListaSubalterniCalificatiPipModel';
import { ListaSubalterniPipDisplayModel, PipDisplayAddEditModel } from 'src/app/models/pip/PlanInbaunatirePerformante';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PipService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  getPipDisplay(idAngajat: number, year: number | null): Observable<PipDisplayAddEditModel> {
    return this.httpClient.get<PipDisplayAddEditModel>(this.baseUrl + "/PIP/display", {
      params: {
        idAngajat: idAngajat,
        year: year ? year : ""
      }
    });
  }

  getListaSubalterniWithPip(currentPage: number, itemsPerPage: number, anul: number | null, filter: string | null, filterFirma: number | null): Observable<ListaSubalterniPipDisplayModel> {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;
    const matricolaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.matricola;

    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage);
    params = params.append("itemsPerPage", itemsPerPage);
    params = params.append("idRol", idRolLoggedInUser ? idRolLoggedInUser : '');
    params = params.append("idFirmaLoggedInUser", idFirmaLoggedInUser ? idFirmaLoggedInUser : '');
    params = params.append("filter", filter ? filter : '');
    params = params.append("filterFirma", filterFirma ? filterFirma : '');
    params = params.append("anul", anul ? anul : '');
    params = params.append("matricolaLoggedInUser", matricolaLoggedInUser ? matricolaLoggedInUser : '');

    return this.httpClient.get<ListaSubalterniPipDisplayModel>(this.baseUrl + "/PIP/listaSubalterni/ongoingPip", { params: params });
  }

  getListaSubalterniWithPipIstoric(currentPage: number, itemsPerPage: number, anul: number | null, filter: string | null, filterFirma: number | null): Observable<ListaSubalterniPipDisplayModel> {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;
    const matricolaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.matricola;

    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage);
    params = params.append("itemsPerPage", itemsPerPage);
    params = params.append("idRol", idRolLoggedInUser ? idRolLoggedInUser : '');
    params = params.append("idFirmaLoggedInUser", idFirmaLoggedInUser ? idFirmaLoggedInUser : '');
    params = params.append("filter", filter ? filter : '');
    params = params.append("filterFirma", filterFirma ? filterFirma : '');
    params = params.append("anul", anul ? anul : '');
    params = params.append("matricolaLoggedInUser", matricolaLoggedInUser ? matricolaLoggedInUser : '');

    return this.httpClient.get<ListaSubalterniPipDisplayModel>(this.baseUrl + "/PIP/listaSubalterni/istoricPip", { params: params });
  }

  getListaSubalterniCalificatiForPIP(currentPage: number, itemsPerPage: number, anul: number | null, filter: string | null, filterFirma: number | null): Observable<ListaSubalterniCalificatiPipDisplayModel> {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;
    const matricolaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.matricola;

    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage);
    params = params.append("itemsPerPage", itemsPerPage);
    params = params.append("idRol", idRolLoggedInUser ? idRolLoggedInUser : '');
    params = params.append("idFirmaLoggedInUser", idFirmaLoggedInUser ? idFirmaLoggedInUser : '');
    params = params.append("filter", filter ? filter : '');
    params = params.append("filterFirma", filterFirma ? filterFirma : '');
    params = params.append("anul", anul ? anul : '');
    params = params.append("matricolaLoggedInUser", matricolaLoggedInUser ? matricolaLoggedInUser : '');

    return this.httpClient.get<ListaSubalterniCalificatiPipDisplayModel>(this.baseUrl + "/PIP/listaSubalterni/clasificati", { params: params });
  }

  getListaSubalterniPentruAprobare(currentPage: number, itemsPerPage: number, anul: number | null, filter: string | null, filterFirma: number | null): Observable<ListaSubalterniCalificatiPipDisplayModel> {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;
    const matricolaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.matricola;


    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage);
    params = params.append("itemsPerPage", itemsPerPage);
    params = params.append("idRol", idRolLoggedInUser ? idRolLoggedInUser : '');
    params = params.append("idFirmaLoggedInUser", idFirmaLoggedInUser ? idFirmaLoggedInUser : '');
    params = params.append("filter", filter ? filter : '');
    params = params.append("filterFirma", filterFirma ? filterFirma : '');
    params = params.append("anul", anul ? anul : '');
    params = params.append("matricolaLoggedInUser", matricolaLoggedInUser ? matricolaLoggedInUser : '');

    return this.httpClient.get<ListaSubalterniCalificatiPipDisplayModel>(this.baseUrl + "/PIP/listaSubalterni/aprobare", { params: params });
  }

  createInitial(idAngajat: number): Observable<PipDisplayAddEditModel> {
    return this.httpClient.get<PipDisplayAddEditModel>(this.baseUrl + "/PIP/createInitial/" + idAngajat);
  }

  addPip(pip: PipDisplayAddEditModel): Observable<PipDisplayAddEditModel> {
    return this.httpClient.post<PipDisplayAddEditModel>(this.baseUrl + "/PIP/add", pip);
  }

  updatePip(pip: PipDisplayAddEditModel): Observable<PipDisplayAddEditModel> {
    return this.httpClient.put<PipDisplayAddEditModel>(this.baseUrl + "/PIP/update", pip);
  }

  getExistingPip(idAngajat: number, year: number | null) {
    return this.httpClient.get<number>(this.baseUrl + "/PIP/existing", {
      params: {
        idAngajat: idAngajat,
        year: year
      }
    });
  }

  actualizareListaSublaterniThatNeedPip(anul: number | null): Observable<any> {
    if (!anul) {
      anul = new Date().getFullYear();
    }
    return this.httpClient.get<any>(`${this.baseUrl}/PIP/actualizare/utilizatoriNeedPip/${anul}`);
  }

  hasMediaForPip(idAngajat: number, year: number | null): Observable<boolean> {
    return this.httpClient.get<boolean>(this.baseUrl + "/PIP/hasMediaForPip", {
      params: {
        idAngajat: idAngajat,
        year: year ? year : ""
      }
    });
  }

  getStareActualaPip(idAngajat: number, year: number | null): Observable<NStariPIP> {
    return this.httpClient.get<NStariPIP>(this.baseUrl + "/PIP/stareActuala", {
      params: {
        idAngajat: idAngajat,
        year: year ? year : ""
      }
    });
  }


  // getDDSubalterniWithPip(matricolaSuperior: string | null) {
  //   return this.httpClient.get<SubalterniDropdown[]>(this.baseUrl + "/PIP/ddSubalterniWithPIP", {
  //     params: {
  //       matricolaSuperior: matricolaSuperior ? matricolaSuperior : ""
  //     }
  //   });
  // }

}