export interface ListaSubalterniCalificatiPipModel {
    idAngajat: number;
    idSuperior: number | null;
    matricola: string;
    matricolaSuperior: string | null;
    numePrenume: string;
    username: string;
    postAngajat: string;
    cor: string;
    hasPipAprobat: boolean;
    starePip: string | null;
}

export interface ListaSubalterniCalificatiPipDisplayModel {
    angajatiPip: ListaSubalterniCalificatiPipModel[];
    pages: number;
    currentPage: number;
}
