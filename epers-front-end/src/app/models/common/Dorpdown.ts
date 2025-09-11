export interface DropdownSelection {
    Id: number;
    Text: string;
    Value: string;
    IdFirma: number | null;
}

export interface AppDropdownSelections {
    DdUseri: DropdownSelection[];
    DdPosturi: DropdownSelection[];
    DdCompartimente: DropdownSelection[];
    DdCompetente: DropdownSelection[];
    DdDivizii: DropdownSelection[];
    DdLocatii: DropdownSelection[];
    DdRoluri: DropdownSelection[];
    DdObiective: DropdownSelection[];
    DdCursuri: DropdownSelection[];
}