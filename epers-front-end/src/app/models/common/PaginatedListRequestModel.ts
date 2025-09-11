export interface PaginatedListRequestModel {
    currentPage: number;
    itemsPerPage: number;
    idRol: number;
    idFirmaLoggedInUser: number | null;
    filter: string | null;
    filterFirma: number | null;
    matricolaLoggedInUser: string | null;
}
