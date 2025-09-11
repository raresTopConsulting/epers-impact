using Epers.DataAccess;
using Epers.Models.Concluzii;
using Epers.Models.Evaluare;
using Epers.Models.Pagination;
using EpersBackend.Services.Common;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Evaluare
{
    public class ConcluziiService : IConcluziiService
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<Evaluare_competente> _logger;
        private readonly IEvaluareService _evaluareService;
        private readonly IConversionHelper _conversionHelper;
        private readonly IPagination _paginationService;


        public ConcluziiService(EpersContext epersContext,
            ILogger<Evaluare_competente> logger,
            IEvaluareService evaluareService,
            IPagination pagination,
            IConversionHelper conversionHelper)
        {
            _epersContext = epersContext;
            _logger = logger;
            _evaluareService = evaluareService;
            _paginationService = pagination;
            _conversionHelper = conversionHelper;
        }

        public void Add(Concluzie concluzie, long[] idEvaluari, string loggedInUserMatricola)
        {
            foreach (var idEval in idEvaluari)
            {
                var evaluare = _evaluareService.Get(idEval);

                evaluare.UpdateDate = DateTime.Now;
                evaluare.UpdateId = loggedInUserMatricola;
                evaluare.ConcluziiAspecteGen = concluzie.ConcluziiAspecteGen;
                evaluare.ConcluziiEvalCantOb = concluzie.ConcluziiEvalCantOb;
                evaluare.ConcluziiEvalCompActDezProf = concluzie.ConcluziiEvalCompActDezProf;
                evaluare.Id_training = concluzie.IdTraining;

                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.Evaluare_competente.Update(evaluare);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cocnluzii: Update - Add Concluzie");
                    throw;
                }
            }
        }
        
        public long[] GetIdsEvaluariSubaltern(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
                anul = DateTime.Now.Year;

            return _epersContext.Evaluare_competente.Where(eval => eval.Anul == anul
                && idAngajat.ToString() == eval.Id_angajat && eval.Flag_finalizat == 1)
                .Select(e => e.Id).ToArray();
        }

        public Concluzie? GetIstoric(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
                anul = DateTime.Now.Year;

            var istoricConcluzii = from evaluare in _epersContext.Evaluare_competente
                                   where evaluare.Id_angajat == idAngajat.ToString() && evaluare.Anul == anul

                                   select new Concluzie
                                   {
                                       Anul = evaluare.Anul,
                                       ConcluziiAspecteGen = evaluare.ConcluziiAspecteGen,
                                       ConcluziiEvalCantOb = evaluare.ConcluziiEvalCantOb,
                                       ConcluziiEvalCompActDezProf = evaluare.ConcluziiEvalCompActDezProf,
                                       IdTraining = evaluare.Id_training,
                                       DisplayIdsTraining = !string.IsNullOrWhiteSpace(evaluare.Id_training) ? _conversionHelper.ConvertStringWithCommaSeparatorToIntArray(evaluare.Id_training) : null
                                   };


            return istoricConcluzii.FirstOrDefault();
        }

        public ConcluziiListaSubalterniDisplayModel GetListaAngajatiAdminHrAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniConcluzii = Array.Empty<ConcluziiListaSubalterni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where idFirmaFilter == user.IdFirma
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where user.IdFirma == idFirmaFilter
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where idFirmaFilter == user.IdFirma && (
                                 user.Matricola.Contains(filter) ||
                                 user.NumePrenume.Contains(filter) ||
                                 post.Nume.Contains(filter) ||
                                 (post.COR != null && post.COR.Contains(filter)))
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where user.IdFirma == idFirmaFilter && (
                                                user.Matricola.Contains(filter) ||
                                                user.NumePrenume.Contains(filter) ||
                                                post.Nume.Contains(filter) ||
                                                (post.COR != null && post.COR.Contains(filter)))
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where user.Matricola.Contains(filter) ||
                                 user.NumePrenume.Contains(filter) ||
                                 post.Nume.Contains(filter) ||
                                 (post.COR != null && post.COR.Contains(filter))
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where
                                                user.Matricola.Contains(filter) ||
                                                user.NumePrenume.Contains(filter) ||
                                                post.Nume.Contains(filter) ||
                                                (post.COR != null && post.COR.Contains(filter))

                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
            }
            return new ConcluziiListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniConcluzii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ConcluziiListaSubalterniDisplayModel GetListaSubalterniAllFirmePaginated(int currentPage, int itemsPerPage,
            string? matricolaSuperior = null, string? filter = null, int? idFirmaFilter = null)
        {
            var listaSubalterniConcluzii = Array.Empty<ConcluziiListaSubalterni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where idFirmaFilter == user.IdFirma && user.MatricolaSuperior == matricolaSuperior
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where idFirmaFilter == user.IdFirma && user.MatricolaSuperior == matricolaSuperior && (
                                 user.Matricola.Contains(filter) ||
                                 user.NumePrenume.Contains(filter) ||
                                 post.Nume.Contains(filter) ||
                                 (post.COR != null && post.COR.Contains(filter)))
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where user.IdFirma == idFirmaFilter && user.MatricolaSuperior == matricolaSuperior && (
                                                user.Matricola.Contains(filter) ||
                                                user.NumePrenume.Contains(filter) ||
                                                post.Nume.Contains(filter) ||
                                                (post.COR != null && post.COR.Contains(filter)))
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where user.MatricolaSuperior == matricolaSuperior
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()
                                                where user.MatricolaSuperior == matricolaSuperior
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = (from user in _epersContext.User
                                 join post in _epersContext.NPosturi
                                 on user.IdPost equals post.Id into postJoin
                                 from post in postJoin.DefaultIfEmpty()
                                 where user.MatricolaSuperior == matricolaSuperior && (user.Matricola.Contains(filter) ||
                                 user.NumePrenume.Contains(filter) ||
                                 post.Nume.Contains(filter) ||
                                 (post.COR != null && post.COR.Contains(filter)))
                                 select user.Id).Count();

                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    listaSubalterniConcluzii = (from user in _epersContext.User
                                                join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                                from post in postJoin.DefaultIfEmpty()

                                                let evaluare = (from eval in _epersContext.Evaluare_competente
                                                                where user.Matricola == eval.Matricola
                                                                orderby eval.Data_EvalFinala descending
                                                                select eval).FirstOrDefault()

                                                where user.MatricolaSuperior == matricolaSuperior && (
                                                user.Matricola.Contains(filter) ||
                                                user.NumePrenume.Contains(filter) ||
                                                post.Nume.Contains(filter) ||
                                                (post.COR != null && post.COR.Contains(filter)))
                                                orderby user.NumePrenume ascending

                                                select new ConcluziiListaSubalterni
                                                {
                                                    IdAngajat = user.Id,
                                                    NumePrenume = user.NumePrenume,
                                                    MatricolaAngajat = user.Matricola,
                                                    COR = post.COR != null ? post.COR : "",
                                                    PostAngajat = post.Nume,

                                                    DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                      evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                    FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                    DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                      evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                    Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                      evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

                }
            }
            return new ConcluziiListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniConcluzii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ConcluziiListaSubalterniDisplayModel GetListaAngajatiAdminHrFirmaPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null)
        {
            var listaSubalterniConcluzii = Array.Empty<ConcluziiListaSubalterni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();


            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = (from user in _epersContext.User
                             join post in _epersContext.NPosturi
                             on user.IdPost equals post.Id into postJoin
                             from post in postJoin.DefaultIfEmpty()
                             where loggedInUserFirma == user.IdFirma
                             select user.Id).Count();

                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniConcluzii = (from user in _epersContext.User
                                            join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                            from post in postJoin.DefaultIfEmpty()

                                            let evaluare = (from eval in _epersContext.Evaluare_competente
                                                            where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                            orderby eval.Data_EvalFinala descending
                                                            select eval).FirstOrDefault()

                                            where user.IdFirma == loggedInUserFirma
                                            orderby user.NumePrenume ascending

                                            select new ConcluziiListaSubalterni
                                            {
                                                IdAngajat = user.Id,
                                                NumePrenume = user.NumePrenume,
                                                MatricolaAngajat = user.Matricola,
                                                COR = post.COR != null ? post.COR : "",
                                                PostAngajat = post.Nume,

                                                DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                  evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                  evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                  evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                            }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

            }
            else
            {
                totalRows = (from user in _epersContext.User
                             join post in _epersContext.NPosturi
                             on user.IdPost equals post.Id into postJoin
                             from post in postJoin.DefaultIfEmpty()
                             where loggedInUserFirma == user.IdFirma && (
                             user.Matricola.Contains(filter) ||
                             user.NumePrenume.Contains(filter) ||
                             post.Nume.Contains(filter) ||
                             (post.COR != null && post.COR.Contains(filter)))
                             select user.Id).Count();

                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniConcluzii = (from user in _epersContext.User
                                            join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                            from post in postJoin.DefaultIfEmpty()

                                            let evaluare = (from eval in _epersContext.Evaluare_competente
                                                            where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                            orderby eval.Data_EvalFinala descending
                                                            select eval).FirstOrDefault()

                                            where user.IdFirma == loggedInUserFirma && (
                                            user.Matricola.Contains(filter) ||
                                            user.NumePrenume.Contains(filter) ||
                                            post.Nume.Contains(filter) ||
                                            (post.COR != null && post.COR.Contains(filter)))
                                            orderby user.NumePrenume ascending

                                            select new ConcluziiListaSubalterni
                                            {
                                                IdAngajat = user.Id,
                                                NumePrenume = user.NumePrenume,
                                                MatricolaAngajat = user.Matricola,
                                                COR = post.COR != null ? post.COR : "",
                                                PostAngajat = post.Nume,

                                                DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                  evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                  evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                  evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                            }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

            }
            return new ConcluziiListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniConcluzii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public ConcluziiListaSubalterniDisplayModel GetListaSubalterniPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? matricolaSuperior = null, string? filter = null)
        {
            var listaSubalterniConcluzii = Array.Empty<ConcluziiListaSubalterni>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();


            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = (from user in _epersContext.User
                             join post in _epersContext.NPosturi
                             on user.IdPost equals post.Id into postJoin
                             from post in postJoin.DefaultIfEmpty()
                             where loggedInUserFirma == user.IdFirma && matricolaSuperior == user.MatricolaSuperior
                             select user.Id).Count();

                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniConcluzii = (from user in _epersContext.User
                                            join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                            from post in postJoin.DefaultIfEmpty()

                                            let evaluare = (from eval in _epersContext.Evaluare_competente
                                                            where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                            orderby eval.Data_EvalFinala descending
                                                            select eval).FirstOrDefault()

                                            where user.IdFirma == loggedInUserFirma && matricolaSuperior == user.MatricolaSuperior
                                            orderby user.NumePrenume ascending

                                            select new ConcluziiListaSubalterni
                                            {
                                                IdAngajat = user.Id,
                                                NumePrenume = user.NumePrenume,
                                                MatricolaAngajat = user.Matricola,
                                                COR = post.COR != null ? post.COR : "",
                                                PostAngajat = post.Nume,

                                                DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                  evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                  evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                  evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                            }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

            }
            else
            {
                totalRows = (from user in _epersContext.User
                             join post in _epersContext.NPosturi
                             on user.IdPost equals post.Id into postJoin
                             from post in postJoin.DefaultIfEmpty()
                             where loggedInUserFirma == user.IdFirma  && matricolaSuperior == user.MatricolaSuperior && (
                             user.Matricola.Contains(filter) ||
                             user.NumePrenume.Contains(filter) ||
                             post.Nume.Contains(filter) ||
                             (post.COR != null && post.COR.Contains(filter)))
                             select user.Id).Count();

                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                listaSubalterniConcluzii = (from user in _epersContext.User
                                            join post in _epersContext.NPosturi on user.IdPost equals post.Id into postJoin
                                            from post in postJoin.DefaultIfEmpty()

                                            let evaluare = (from eval in _epersContext.Evaluare_competente
                                                            where user.Matricola == eval.Matricola && user.IdFirma == eval.IdFirma
                                                            orderby eval.Data_EvalFinala descending
                                                            select eval).FirstOrDefault()

                                            where user.IdFirma == loggedInUserFirma && matricolaSuperior == user.MatricolaSuperior && (
                                            user.Matricola.Contains(filter) ||
                                            user.NumePrenume.Contains(filter) ||
                                            post.Nume.Contains(filter) ||
                                            (post.COR != null && post.COR.Contains(filter)))
                                            orderby user.NumePrenume ascending

                                            select new ConcluziiListaSubalterni
                                            {
                                                IdAngajat = user.Id,
                                                NumePrenume = user.NumePrenume,
                                                MatricolaAngajat = user.Matricola,
                                                COR = post.COR != null ? post.COR : "",
                                                PostAngajat = post.Nume,

                                                DataEvalFin = evaluare.Data_EvalFinala.HasValue ?
                                                  evaluare.Data_EvalFinala.Value.ToString("dd/MM/yyyy") : "",

                                                FlagFinalizat = evaluare.Flag_finalizat == 1 ? true : false,

                                                DataUltimaEval = evaluare.UpdateDate.HasValue ?
                                                  evaluare.UpdateDate.Value.ToString("dd/MM/yyyy") : "",

                                                Concluzii = string.Concat(evaluare.ConcluziiAspecteGen,
                                                  evaluare.ConcluziiEvalCantOb, evaluare.ConcluziiEvalCompActDezProf)
                                            }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

            }
            return new ConcluziiListaSubalterniDisplayModel
            {
                ListaSubalterni = listaSubalterniConcluzii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }
    }
}

