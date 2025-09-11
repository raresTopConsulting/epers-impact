export interface NCursuri {
    id: number;
    denumire: string;
    organizator: string;
    pret: number | null;
    dataInceput: string | null;
    dataSfarsit: string | null;
    locatie: string;
    isOnline: boolean | null;
    link: string;
    activ: boolean;
    idFirma: number | null;
    firma: string;
}
