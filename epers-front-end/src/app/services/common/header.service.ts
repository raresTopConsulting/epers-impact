import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AfisareUserDetails } from 'src/app/models/common/UserDetails';
import { AfisareHeaderModel } from 'src/app/models/common/UserHeader';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HeaderService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  getHeaderData(idAngajat: number): Observable<AfisareHeaderModel> {
    return this.httpClient.get<AfisareHeaderModel>(this.baseUrl + "/Header/" + idAngajat);
  }

  getUserDetails(id: number): Observable<AfisareUserDetails> {
    return this.httpClient.get<AfisareUserDetails>(this.baseUrl + "/Header/userDetails/" + id);
  }
}
