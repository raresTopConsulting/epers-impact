import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class EmailPipService {
    baseUrl = environment.baseUrl;

    constructor(private httpClient: HttpClient) { }

    sendEmailCalificatPipToHRandAngajat(idAngajat: number, idFrima: number | null) {
        this.httpClient.get(this.baseUrl + "/EmailPip/calificat?idAngajat=" + idAngajat + "&idFrima=" + idFrima).subscribe();
    }

    sendEmailPipAprobatToAgajatAngManager(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailPip/aprobat?idAngajat=" + idAngajat).subscribe();
    }

    sendEmailPipRespinsToAngajatAndManager(idAngajat: number) {
        this.httpClient.get(this.baseUrl + "/EmailPip/respins?idAngajat=" + idAngajat).subscribe();
    }

    sendEmaiPipStatusUpdate(idAngajat: number, idFrima: number | null) {
        this.httpClient.get(this.baseUrl + "/EmailPip/statusUpdate?idAngajat=" + idAngajat  + "&idFrima=" + idFrima).subscribe();
    }

    sendEmaiPipIncheiat(idAngajat: number, idFrima: number | null) {
        this.httpClient.get(this.baseUrl + "/EmailPip/incheiat?idAngajat=" + idAngajat  + "&idFrima=" + idFrima).subscribe();
    }
}