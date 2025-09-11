export interface SetareProfil {
    id: number;
    id_Skill: number | null;
    id_Post: number | null;
    ideal: number | null;
}

export interface TableSetareProfil {
    setareProfil: SetareProfil;
    denumireSkill: string;
    descriereSkill: string;
    selected: boolean;
}

