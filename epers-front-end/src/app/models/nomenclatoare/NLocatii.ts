export interface NLocatii {
    id: number;
    denumire: string;
    idFirma: number | null;
    isSediuPrincipalFirma: boolean | null;
    adresa: string | null;
    localitate: string | null;
    judet: string | null;
    tara: string | null;
    dataIn: Date | null;
    dataSf: Date | null;
    activ: boolean;
    firma: string;
}