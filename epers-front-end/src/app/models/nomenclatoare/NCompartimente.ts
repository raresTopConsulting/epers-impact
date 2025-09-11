export interface NCompartimente {
    id: number;
    denumire: string;
    id_Locatie: number | null;
    data_in: string | null;
    data_sf: string | null;
    sus: number | null;
    jos: number | null;
    subCompartiment: string;
    activ: boolean;
    firma: string;
    idFirma: number | null;
}

export interface NCompartimentDisplay extends NCompartimente {
    locatie: string | null;
}

export interface CompartimentSelection {
    id: number;
    denumire: string;
    idFirma: number | null;
}