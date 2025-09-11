import { ObiectivTemplate } from "./Obiective";

export interface SetareObiective {
    idAngajat: string;
    matricolaAngajat: string;
    idSuperior: string;
    matricolaSuperior: string;
    idCompartiment: number | null;
    idPost: number | null;
    updateId: string;
    updateDate: Date | null;
    obiectivTemplate: ObiectivTemplate[];
}