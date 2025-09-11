export interface NFirme {
    id: number;
    denumire: string;
    codFiscal: string;
    tipIntreprindere: string;
    atributFiscal: string;
    dataIn: Date | null;
    dataSf: Date | null;
    adresa: string | null;
    activ: boolean;
}