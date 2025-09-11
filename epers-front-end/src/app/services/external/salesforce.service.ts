import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NCompartimentDisplay, NCompartimente } from 'src/app/models/nomenclatoare/NCompartimente';
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

  public sync(compartiment: NCompartimente) {
    return this.httpClient.put<NCompartimente>(this.baseUrl + "/Salesforce/Agent/SyncAll/", compartiment);
  }
}
