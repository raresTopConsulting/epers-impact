using Epers.Models.Concluzii;

namespace EpersBackend.Services.Evaluare
{
    public interface IConcluziiService
    {
        ConcluziiListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null);

        ConcluziiListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaSuperior = null, string? filter = null, int? idFirmaFilter = null);

        ConcluziiListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null);

        ConcluziiListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? matricolaSuperior = null, string? filter = null);

        Concluzie? GetIstoric(int idAngajat, int? anul = null);

        long[] GetIdsEvaluariSubaltern(int idAngajat, int? anul = null);

        void Add(Concluzie concluzie, long[] idEvaluari, string loggedInUserMatricola);
    }
}

