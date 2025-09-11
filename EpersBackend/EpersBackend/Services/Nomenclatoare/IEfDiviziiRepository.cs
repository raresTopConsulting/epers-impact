using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public interface IEfDiviziiRepository
    {
        NDivizii Get(int id);
       DiviziiDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        DiviziiDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        void Update(NDivizii divizie);
        void Add(NDivizii divizie);
        void SetToInactive(int id);
    }
}

