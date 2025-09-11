export interface EvaluareListaSubalteni {
    numePrenume: string;
    idAngajat: number;
    matricolaAngajat: string;
    postAngajat: string;
    cor: string;
    dataUltimaEval: string;
    dataEvalSef: string;
    dataAutoEval: string;
    dataEvalFin: string;
    flagFinalizat: boolean;
    concluzii: string;
    finaliztAnulCurent: boolean;
}

export interface EvaluareListaSubalterniDisplayModel {
    listaSubalterni: EvaluareListaSubalteni[];
    pages: number;
    currentPage: number;
}
