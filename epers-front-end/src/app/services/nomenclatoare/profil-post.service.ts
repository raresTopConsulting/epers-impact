import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SetareProfil } from 'src/app/models/nomenclatoare/SetareProfil';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProfilPostService {
  baseUrl = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }
  
  getProfilulPostului(id: number): Observable<SetareProfil> {
    return this.httpClient.get<SetareProfil>(this.baseUrl + "/ProfilPostController/" + id);
  }
}
