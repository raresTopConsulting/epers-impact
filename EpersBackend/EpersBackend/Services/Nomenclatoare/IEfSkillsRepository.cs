using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
    public interface IEfSkillsRepository
    {
        NSkills Get(int id);
        SkillsDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null);
        SkillsDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null);
        void Update(NSkills skill);
        void Add(NSkills skill);
        void SetToInactive(int id);
    }
}

