import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExportRapoarteService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  exortEvaluariToPdfAndExcel(anul: number): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/ExportEvaluari/all?anul=" + anul);
  }

  exortEvaluariToPdfAndExcelForAngajatiBetween(anul: number, idStart: number, idStop: number): Observable<any> {
    let params = new HttpParams();
    params = params.append("idStart", idStart);
    params = params.append("idStop", idStop);
    params = params.append("anul", anul ? anul: '');

    return this.httpClient.get<any>(this.baseUrl + "/ExportEvaluari/between", { params: params });
  }

}
