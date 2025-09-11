export interface NPosturi {
    id: number;
    nume: string;
    idFirma: number | null;
    dataIn: Date | null;
    dataSf: Date | null;
    profilCompetente: string | null;
    cor: string | null;
    denFunctie: string | null;
    nivelPost: string | null;
    punctaj: number | null;
    activ: boolean;
    firma: string | null;
}
