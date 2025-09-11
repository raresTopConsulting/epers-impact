using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public interface IEfCompartimenteRepository
    {
        NCompartimente Get(int id);
        CompartimenteDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        CompartimenteDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        void Update(NCompartimente compartiment);
        void Add(NCompartimente compartiment);
        void SetToInactive(int id);
    }
}

