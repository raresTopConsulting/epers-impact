using Epers.Models.Afisare;
using Epers.Models.Evaluare;

namespace EpersBackend.Services.Evaluare
{
    public interface IEvaluareService
    {
        EvaluareListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null);

        EvaluareListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaSuperior = null, string? filter = null, int? idFirmaFilter = null);

        EvaluareListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null);

        EvaluareListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? matricolaSuperior = null, string? filter = null);

        void UpsertEvaluare(EvaluareTemplate evalTemplate);

        void AddIdFrima(Evaluare_competente evalCompetente);

        AfisareSkillsEvalModel GetEvalTemplate(int idAngajat, int? anul = null);

        AfisareSkillsEvalModel GetIstoricEvalTemplate(int idAngajat, int? anul = null);

        AfisareEvalCalificativFinal GetIstoricEvalCalificativFinal(int idAngajat, int? anul = null);

        void ContestareEvaluare(int idAngajat, int? anul = null);

        Evaluare_competente Get(long id);

        List<Evaluare_competente> GetAllForUser(string idAngajat);

        EvaluarePipData GetDateEvaluareForPip(int idAngajat, int? anul = null);

        string UpdateCalificativFinalEvaluare(int? anul = null);

        bool UserHasEvaluareFinalizata(int idAngajat, int anul);
    }
}
