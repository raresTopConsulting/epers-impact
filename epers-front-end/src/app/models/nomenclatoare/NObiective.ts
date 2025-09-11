export interface NObiective {
    id: number;
    denumire: string;
    idFirma: number | null;
    idCompartiment: number | null;
    idPost: number | null;
    valMin: number | null;
    valTarget: number | null;
    valMax: number | null;
    bonusMin: number | null;
    bonusTarget: number | null;
    bonusMax: number | null;
    frecventa: string;
    firma: string;
    pondere: number | null;
    isFaraProcent: boolean;
    tip: string;
    updateId: string;
    updateDate: Date | null;
    isBonusProcentual: boolean | null;
}

export interface NObiectiveDisplayModel {
    nomObiectiveData: NObiective[];
    pages: number;
    currentPage: number;
}


export interface AfisareNObiective {
    id: number;
    post: string;
    compartiment: string;
    denumire: string;
    tip: string;
    valTarget: number | null;
    valMin: number | null;
    valMax: number | null;
    bonusTarget: number | null;
    bonusMin: number | null;
    bonusMax: number | null;
    frecventa: string | null;
    isFaraProcent: boolean;
    firma: string;
    idFirma: number | null;
}

export interface AfisareNObiectiveDisplayModel {
    afisNomObiectiveData: AfisareNObiective[];
    pages: number;
    currentPage: number;
}