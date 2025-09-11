export interface EvaluareTemplate {
    numePrenume: string;
    matricola: string;
    idAngajat: number;
    idSuperior: number;
    numePrenumeSuperior: string;
    matricolaSuperior: string;
    idPost: number;
    tipEvaluare: number;
    displayIdsTraining: number[];
    dateEval: EvaluareCreateModel[];
    calificativFinal: number | null;
    incadrareCalificativFinal: string | null;
    anul: number;
    flagFinalizat: boolean | null;
}

export interface EvaluareCreateModel {
    denumireSkill: string;
    detaliiSkill: string;
    idSkill: number;
    ideal: number;
    valIndiv: number | null;
    val: number | null;
    valFin: number | null;
    obs: string | null;
}
