using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
    public interface IEfPosturiRepository
    {
        NPosturi Get(int id);
        PosturiDisplayModel GetAllForAllFirmePaginated (int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null);
        PosturiDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        void Update(NPosturi post);
        void Add(NPosturi post);
        void SetToInactive(int id);
    }
}

