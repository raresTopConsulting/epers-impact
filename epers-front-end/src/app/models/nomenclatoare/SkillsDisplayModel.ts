import { NSkills } from "./NSkills";

export interface SkillsDisplayModel {
    skills: NSkills[];
    pages: number;
    currentPage: number;
}
