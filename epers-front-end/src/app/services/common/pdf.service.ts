import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AfisareUserDetails } from 'src/app/models/common/UserDetails';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PdfService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  getTestPdf(): Observable<any> {
    return this.httpClient.get(this.baseUrl + "/PDF/test");
  }
  
  getEvaluarePdf(idAngajat: number, anul: number | null): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/PDF/evaluare", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  getEvaluareConcluziiPdf(idAngajat: number, anul: number | null): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/PDF/evaluare/concluzii", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  getObiectiveIstoricPdf(idAngajat: number, anul: number | null): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/PDF/obiective/istoric", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  getObiectiveActualePdf(idAngajat: number, anul: number | null): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/PDF/obiective/actuale", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  getPipPdf(idAngajat: number, anul: number | null): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + "/PDF/PIP", {
      params: {
        idAngajat: idAngajat,
        anul: anul ? anul : ""
      }
    });
  }

  base64ToBlob(base64String: string, contentType: string): Blob {
    const byteCharacters = atob(base64String);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: contentType });
  }
 
  getUserDetails(id: number): Observable<AfisareUserDetails> {
    return this.httpClient.get<AfisareUserDetails>(this.baseUrl + "/Header/userDetails/" + id);
  }
}
