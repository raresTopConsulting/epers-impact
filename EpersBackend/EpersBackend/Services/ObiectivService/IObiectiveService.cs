using Epers.Models.Obiectiv;

namespace EpersBackend.Services.ObiectivService
{
    public interface IObiectiveService
    {
        Obiective[] GetObiectiveActuale(int idAngajat, string? filter = null);

        Obiective[] GetIstoricObiective(int idAngajat, string? filter = null);

        void Add(SetareObiective setareObiective, int[]? idAngajatiSelectati = null);

        ObiectiveListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null);

        ObiectiveListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaSuperior = null, string? filter = null, int? idFirmaFilter = null);

        ObiectiveListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null);

        ObiectiveListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? matricolaSuperior = null, string? filter = null);

        void Evaluare(Obiective[] obiective);

        void InsertObiectiv(Obiective obieciv);
        void UpdateObiectiv(Obiective obieciv);
        void InsertOrUpdateObiectiv(Obiective obieciv);
    }
}

