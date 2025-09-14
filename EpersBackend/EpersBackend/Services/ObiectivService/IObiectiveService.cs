using Epers.Models.Obiectiv;
using Epers.Models.Users;

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

        int GetSalesforceDataInObiective();

        Obiective? GetObActiveForSalesforce(int idAngajat, DateTime dataIn, string denumire);
    }
}

