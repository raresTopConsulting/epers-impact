import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SalesforceService {

constructor(private httpClient: HttpClient) {}
  baseUrl = environment.baseUrl;

  public getAll(): Observable<number> {
    return this.httpClient.get<number>(this.baseUrl + "/Salesforce/Agent/SyncAll");
  }

  // public sync() {
  //   return this.httpClient.post<AgentMetrics>(this.baseUrl + "/Salesforce/Agent/SyncAll/", {});
  // }
}
