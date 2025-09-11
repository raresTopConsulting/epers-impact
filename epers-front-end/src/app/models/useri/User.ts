export interface LoggedInUserData {
    id: number;
    idPost: number | null;
    idLocatie: number | null;
    idCompartiment: number | null;
    idSuperior: number | null;
    idFirma: number | null;
    idRol: number;
    matricola: string;
    rol: string;
    numePrenume: string;
    username: string;
    matricolaSuperior: string;
    numeSuperior: string;
    expiresIn: number;
}

export interface UserAuthenticationRequest {
    username: string;
    password: string;
}

export interface UserCreateModel {
    id: number;
    username: string;
    password: string;
    confirmPassword: string;
    numePrenume: string;
    matricola: string;
    idRol: number;
    idPost: number | null;
    idLocatie: number | null;
    idCompartiment: number | null;
    idSuperior: number | null;
    idFirma: number | null;
    matricolaSuperior: string;
}

export interface UserEditModel {
    id: number;
    username: string;
    numePrenume: string;
    matricola: string;
    idRol: number;
    idPost: number | null;
    idLocatie: number | null;
    idCompartiment: number | null;
    idFirma: number | null;
    idSuperior: number | null;
    matricolaSuperior: string;
}

export interface ListaUtilizatori {
    id: number;
    idFrima: number | null;
    matricola: string;
    nume_Prenume: string;
    rol: string;
    username: string;
    denumirePost: string;
    cor: string;
    organizatieIntermediara: string;
    organizatieBaza: string;
    locatie: string;
    firma: string;
}

export interface ListaUtilisatoriDisplayModel {
    utilizatori: ListaUtilizatori[];
    pages: number;
    currentPage: number;
}

export interface PasswordChange {
    userId: number;
    username: string;
    newPass: string;
    confNewPass: string;
}

export interface SubalterniDropdown {
    idAngajat: number;
    matricolaAngajat: string;
    numePrenume: string;
    postAngajat: string;
    cor: string;
    hideStartPip: boolean | null;
}