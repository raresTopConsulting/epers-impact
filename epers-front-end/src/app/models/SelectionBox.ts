export interface SelectionBox {
    Id: number;
    Sectiune: string;
    Valoare: string;
}

export interface SelectionBoxes {
    JudeteSelection: SelectionBox[];
    TipCompetenteSelection: SelectionBox[];
    FrecventaObiectiveSelection: SelectionBox[];
    TipObiectiveSelection: SelectionBox[];
}
