import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserDbSetupService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  addAdmin() {
    return this.httpClient.get(this.baseUrl + "/UserDbSetup/addAdmin");
  }

  updateUserDataFromImport24() {
    return this.httpClient.get(this.baseUrl + "/UserDbSetup/import24");
  }

  setHiearchy() {
    return this.httpClient.get(this.baseUrl + "/UserDbSetup/setHierarchy");
  }

  addIdFirmaToEvaluareCompetente() {
    return this.httpClient.get(this.baseUrl + "/UserDbSetup/addIdFirmaToEvaluareCompetente");
  }

}
