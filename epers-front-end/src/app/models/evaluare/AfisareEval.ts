export interface AfisareSkillsEvalModel {
    idPost: number;
    flagFinalizat: boolean;
    dateEval: EvalArray[];
}

export interface EvalArray {
    idSkill: number;
    denumireSkill: string;
    detaliiSkill: string;
    ideal: number;
    valIndiv: number | null;
    val: number | null;
    valFin: number | null;
    obs: string;
    dataEvalFinala: Date | null;
}

export interface AfisareEvalCalificativFinal {
    calificativFinal: number | null;
    incadrareCalificativFinal: string;
}

export const NoteMinimePip = {
    CALIFICATIV_MINIM_INSUFICIENT: 1.5,
    CALIFICATIV_MINIM_NECESITA_EFORT: 2.5
}
