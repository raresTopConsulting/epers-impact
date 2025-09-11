export interface PipDisplayAddEditModel extends PlanInbunatatirePerformante {
    numePrenumeAngajat: string;
    numePrenumeSuperior: string;
    postAngajat: string;
    postSuperior: string | null;
}

export interface PlanInbunatatirePerformante {
    id: number;
    idAngajat: number | null;
    matricola: string | null;
    idSuperior: number | null;
    matricolaSuperior: string | null;
    idPost: number | null;
    idPostSuperior: number | null;
    dataInceputEvaluare: Date | null;
    dataSfarsitEvaluare: Date | null;
    calificativEvaluare: number;
    dataInceputPip: Date;
    dataSfarsitPip: Date;
    calificativMinimPip: number;
    obiectiveDezvoltare: string | null;
    actiuniSalariat: string | null;
    suportManager: string | null;
    altSuport: string | null;
    calificativFinalPip: number | null;
    observatiFinalPip: string | null;
    deczieFinalaManager: string | null;
    idStare: number;
    observatiiHr: string;
    anul: number;
    updateDate: Date;
    updateId: string;
}

export interface ListaSubalterniPipModel {
    idPip: number | null;
    idAngajat: number;
    idSuperior: number | null;
    matricola: string;
    matricolaSuperior: string | null;
    numePrenume: string;
    username: string;
    postAngajat: string;
    cor: string;
    idStarePIP: number;
    denumireStarePIP: string;
}

export interface ListaSubalterniPipDisplayModel {
    angajatiPip: ListaSubalterniPipModel[];
    pages: number;
    currentPage: number;
}

