import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class EmailEvaluareService {
    baseUrl = environment.baseUrl;

    constructor(private httpClient: HttpClient) { }

    sendEmailAutoevaluare(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailEvaluare/autoevaluare?idAngajat=" + idAngajat).subscribe();
    }

    sendEmailEvaluareSublatern(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailEvaluare/evaluareSublatern?idAngajat=" + idAngajat).subscribe();
    }

    sendEmailEvaluareFinala(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailEvaluare/evaluareFinalaSubaltern?idAngajat=" + idAngajat).subscribe();
    }
}