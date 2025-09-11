using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
    public interface IEfCursuriRepository
    {
        NCursuri Get(int id);
        CursuriDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        CursuriDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        void Update(NCursuri curs);
        void Add(NCursuri curs);
        void SetToInactive(int id);
    }
}

