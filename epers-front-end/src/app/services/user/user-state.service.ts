import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HeaderTableFiltersModel } from 'src/app/models/common/HeaderTableFiltersModel';
import { ListaUtilisatoriDisplayModel, LoggedInUserData, SubalterniDropdown, UserCreateModel, UserEditModel } from 'src/app/models/useri/User';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserStateService {
  baseUrl = environment.baseUrl;
  loggedInUserSubject: BehaviorSubject<LoggedInUserData> = new BehaviorSubject<LoggedInUserData>(null);
  loggedInUser$ = this.loggedInUserSubject.asObservable();
  
  private tableFiltersSubject: BehaviorSubject<HeaderTableFiltersModel> = new BehaviorSubject<HeaderTableFiltersModel>({
    filter: '',
    filterFirma: null,
    currentPage: 1,
    itemsPerPage: 10 
  });
  private usersListDisplayModelSubject: BehaviorSubject<ListaUtilisatoriDisplayModel> = new BehaviorSubject<ListaUtilisatoriDisplayModel>(null);

  tableFiltersObs$ = this.tableFiltersSubject.asObservable();
  userListDisplayModelObs$ = this.usersListDisplayModelSubject.asObservable();

  constructor(private httpClient: HttpClient) { }

  register(registerUser: UserCreateModel) {
    return this.httpClient.post(this.baseUrl + "/User/register", registerUser);
  }

  updateUserData(userEdit: UserEditModel) {
    return this.httpClient.put(this.baseUrl + "/User/updateUserData", userEdit);
  }

  listaUtilizatori(currentPage: number, itemsPerPage: number, filter: string, filterFirma: number | null) {
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

    return this.httpClient.get<ListaUtilisatoriDisplayModel>(this.baseUrl + "/User/listaUtilizatori", { params: params }).pipe(tap((data) => {
      this.usersListDisplayModelSubject.next(data);
    })).subscribe();
  }

  get(id: number): Observable<UserCreateModel> {
    return this.httpClient.get<UserCreateModel>(this.baseUrl + "/User/" + id);
  }

  changePassword(id: number, password: string) {
    return this.httpClient.put(this.baseUrl + "/User/changePassword", { id: id, password: password });
  }

  delete(id: number) {
    return this.httpClient.delete(this.baseUrl + "/User/" + id);
  }

  registerFromSincron(minId: number, maxId: number) {
    return this.httpClient.get(this.baseUrl + "/User/registerSincron", {
      params: {
        minId: minId,
        maxId: maxId
      }
    });
  }

  getDropdownSubalterni(): Observable<SubalterniDropdown[]> {
    let loggedInUserMatricola = JSON.parse(localStorage.getItem('user')).matricola;
    let loggedInUserIdRol = JSON.parse(localStorage.getItem('user')).idRol;

    return this.httpClient.get<SubalterniDropdown[]>(this.baseUrl + "/User/dropdownSublaterni", {
      params: {
        userRolId: loggedInUserIdRol,
        loggedInUserMatricola: loggedInUserMatricola
      }
    });
  }

  upsertUserDataWithLocalStorage() {
    let localStorageUserData = JSON.parse(localStorage.getItem("user")) as unknown as LoggedInUserData;
    this.loggedInUserSubject.next(localStorageUserData);
  }

}
