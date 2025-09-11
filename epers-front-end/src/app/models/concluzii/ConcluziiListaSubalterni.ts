export interface ConcluziiListaSubalterni {
    numePrenume: string;
    idAngajat: number;
    matricolaAngajat: string;
    postAngajat: string;
    cor: string;
    dataUltimaEval: string;
    dataEvalFin: string;
    flagFinalizat: boolean;
    concluzii: string;
}

export interface ConcluziiListaSubalterniDisplayModel {
    listaSubalterni: ConcluziiListaSubalterni[];
    pages: number;
    currentPage: number;
}
