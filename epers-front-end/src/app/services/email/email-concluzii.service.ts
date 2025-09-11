import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class EmailConcluziiService {
    baseUrl = environment.baseUrl;

    constructor(private httpClient: HttpClient) { }

    sendEmailConcluzii(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailConcluzii/?idAngajat=" + idAngajat).subscribe();
    }

}