import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class EmailObiectiveService {
    baseUrl = environment.baseUrl;

    constructor(private httpClient: HttpClient) { }

    sendEmailObiectiveSelectate(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailObiective/obiectiveSetate?idAngajat=" + idAngajat).subscribe();
    }

    sendEmailObiectiveEvaluate(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailObiective/obiectiveEvaluate?idAngajat=" + idAngajat).subscribe();
    }
}