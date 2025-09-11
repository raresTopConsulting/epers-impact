import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { NPosturi } from 'src/app/models/nomenclatoare/NPosturi';
import { PosturiDisplayModel } from 'src/app/models/nomenclatoare/PosturiDisplayModel';
import { SetareProfil, TableSetareProfil } from 'src/app/models/nomenclatoare/SetareProfil';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PosturiService {
  baseUrl = environment.baseUrl;
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private posturiDisplayModelSubject: BehaviorSubject<PosturiDisplayModel> = new BehaviorSubject<PosturiDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  posturiDisplayModelObs$ = this.posturiDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  getAllPaginated(currentPage: number, itemsPerPage: number, filter: string, filterFirma: number | null) {
    const idFirmaLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idFirma;
    const idRolLoggedInUser = JSON.parse(localStorage.getItem('user'))?.idRol;

    // PaginatedListRequestModel
    let params = new HttpParams();
    params = params.append("currentPage", currentPage ? currentPage : 1);
    params = params.append("itemsPerPage", itemsPerPage ? itemsPerPage: 10);
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

    this.httpClient.get<PosturiDisplayModel>(this.baseUrl + "/Posturi", { params: params }).pipe(tap((data) => {
      this.posturiDisplayModelSubject.next(data);
    })).subscribe();
  }

  get(id: number): Observable<NPosturi> {
    return this.httpClient.get<NPosturi>(this.baseUrl + "/Posturi/" + id);
  }

  update(post: NPosturi) {
    return this.httpClient.put<NPosturi>(this.baseUrl + "/Posturi", post);
  }

  addProfilPost(profil: SetareProfil) {
    return this.httpClient.post(this.baseUrl + "/Posturi/addProfilPost", profil);
  }

  updateProfilPost(profil: SetareProfil) {
    return this.httpClient.put(this.baseUrl + "/Posturi/updateProfilPost", profil);
  }

  deleteProfiliPost(idProfilPost: number) {
    return this.httpClient.delete(this.baseUrl + "/Posturi/deleteProfiliPost/" + idProfilPost);
  }

  getProfilPost(idPost: number): Observable<TableSetareProfil[]> {
    return this.httpClient.get<TableSetareProfil[]>(this.baseUrl + "/Posturi/getProfilPost/" + idPost);
  }

  add(post: NPosturi) {
    return this.httpClient.post(this.baseUrl + "/Posturi", post);
  }

  delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/Posturi/" + id);
  }

}
