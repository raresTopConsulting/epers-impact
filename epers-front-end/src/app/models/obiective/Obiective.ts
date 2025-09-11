import { SubalterniDropdown } from "../useri/User";

export interface ObiectiveListaSubalterniDisplayModel {
    listaSubalterni: SubalterniDropdown[];
    pages: number;
    currentPage: number;
}

export interface ObiectivTemplate {
    id: number;
    denumire: string;
    valMin: number | null;
    valTarget: number | null;
    valMax: number | null;
    bonusMin: number | null;
    bonusTarget: number | null;
    bonusMax: number | null;
    isFaraProcent: boolean;
    tip: string;
    dataIn: Date | null;
    dataSf: Date | null;
    frecventa: string;
    isBonusProcentual: boolean | null;
}

export interface Obiective {
    id: number;
    denumire: string;
    idAngajat: string;
    matricolaAngajat: string;
    idSuperior: string;
    matricolaSuperior: string;
    idCompartiment: number | null;
    idPost: number | null;
    dataIn: Date | null;
    dataSf: Date | null;
    valMin: number | null;
    valTarget: number | null;
    valMax: number | null;
    bonusMin: number | null;
    bonusTarget: number | null;
    bonusMax: number | null;
    frecventa: string;
    tip: string;
    valoareRealizata: string;
    isRealizat: boolean;
    pondere: number | null;
    isActive: boolean | null;
    isFaraProcent: boolean;
    updateId: string;
    updateDate: Date | null;
    isBonusProcentual: boolean | null;
}

export interface ObiectiveDisplayModel {
    listaObActuale: Obiective[];
    pages: number;
    currentPage: number;
}
