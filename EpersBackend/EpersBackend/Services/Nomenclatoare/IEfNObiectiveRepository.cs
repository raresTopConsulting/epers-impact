using Epers.Models.Afisare;
using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
    public interface IEfNObiectiveRepository
    {
        NObiective Get(int id);
        AfisareNObiectiveDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        AfisareNObiectiveDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        void Update(NObiective obiectiv);
        void Add(NObiective obiectiv);
    }
}

