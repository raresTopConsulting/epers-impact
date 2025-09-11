export interface AfisareHeaderModel {
    idSubaltern: number | null;
    idSuperior: number | null;
    numePrenume: string;
    matricola: string;
    numePrenumeSef: string | null;
    matricolaSef: string | null;
    denumirePost: string;
    cor: string | null;
    compartiment: string;
    locatie: string;
    denumirePostSupervizor: string;
    corSupervizor: string | null;
    compartimentSupervizor: string;
    locatieSupervizor: string;
    idCompartiment: number | null;
    idCompartimentSuperior: number | null;
    idPost: number | null;
    idPostSuperior: number | null;
    idLocatie: number | null;
    idLocatieSupervizor: number | null;
}
