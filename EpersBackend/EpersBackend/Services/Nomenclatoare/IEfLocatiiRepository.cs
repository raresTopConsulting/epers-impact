using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
    public interface IEfLocatiiRepository
    {
        NLocatii Get(int id);
        LocatiiDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        LocatiiDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        NLocatii[] GetAll();
        void Update(NLocatii locatie);
        void Add(NLocatii locatie);
        void SetToInactive(int id);
    }
}

