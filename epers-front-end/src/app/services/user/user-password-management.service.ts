import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PasswordChange } from 'src/app/models/useri/User';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserPasswordManagementService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  changePassword(changePassword: PasswordChange) {
    return this.httpClient.put(this.baseUrl + "/UserPasswordManagement/changePassword", changePassword);
  }

  forgotPassword(email: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpClient.post(this.baseUrl + "/UserPasswordManagement/forgotPassword", JSON.stringify(email), { headers });
  }

  resetPassword(changePassword: PasswordChange) {
    return this.httpClient.put(this.baseUrl + "/UserPasswordManagement/resetPassword", changePassword);
  }
}
