using Epers.DataAccess;
using Epers.Models.Afisare;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Common;
using EpersBackend.Services.Pagination;
namespace EpersBackend.Services.Nomenclatoare
{
    public class EfNObiectiveRepository : IEfNObiectiveRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IEfPosturiRepository _postRepository;
        private readonly IEfCompartimenteRepository _compartimentRepository;
        private readonly IEfSelectionBoxRepository _efSelectionBoxRepository;
        private readonly ILogger<NObiective> _logger;
        private readonly IPagination _paginationService;

        public EfNObiectiveRepository(EpersContext epersContext,
             ILogger<NObiective> logger,
             IEfPosturiRepository efPosturiRepository,
             IEfCompartimenteRepository compartimenteRepository,
             IEfSelectionBoxRepository efSelectionBoxRepository,
             IPagination paginationService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _postRepository = efPosturiRepository;
            _compartimentRepository = compartimenteRepository;
            _efSelectionBoxRepository = efSelectionBoxRepository;
            _paginationService = paginationService;
        }

        public void Add(NObiective obiectiv)
        {
            obiectiv.UpdateDate = DateTime.Now;

            if (obiectiv.IsBonusProcentual == true)
            {
                CalculBonusProcentual(obiectiv);
            }

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NObiective.Add(obiectiv);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfNObiectiveRepository: Add");
                throw;
            }
        }

        public void Update(NObiective obiectiv)
        {
            try
            {
                if (obiectiv.IsBonusProcentual == true)
                {
                    CalculBonusProcentual(obiectiv);
                }

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NObiective.Update(obiectiv);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfNObiectiveRepository: Update");
                throw;
            }
        }

        public NObiective Get(int id)
        {
            try
            {
                return _epersContext.NObiective.Single(nob => nob.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfNObiectiveRepository: Get");
                throw;
            }
        }

        public AfisareNObiectiveDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null)
        {
            var afisareNObiective = Array.Empty<AfisareNObiective>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();
            var selBoxTipuriOb = _efSelectionBoxRepository.GetTipuriObiective();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NObiective.Count(ob => ob.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var obiectiveQuery = from nOb in _epersContext.NObiective
                                         join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                         join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                         from post in postJoin.DefaultIfEmpty()
                                         where nOb.IdFirma == idFirmaFilter
                                         select new AfisareNObiective
                                         {
                                             Id = nOb.Id,
                                             IsFaraProcent = nOb.IsFaraProcent,
                                             ValMax = nOb.ValMax,
                                             ValMin = nOb.ValMin,
                                             ValTarget = nOb.ValTarget,
                                             BonusMax = nOb.BonusMax,
                                             BonusMin = nOb.BonusMin,
                                             BonusTarget = nOb.BonusTarget,
                                             Denumire = nOb.Denumire,
                                             Frecventa = nOb.Frecventa,
                                             IdFirma = nOb.IdFirma,
                                             IsBonusProcentual = nOb.IsBonusProcentual,
                                             Firma = firma.Denumire,
                                             Tip = nOb.Tip,
                                             Post = post.Nume
                                         };
                    afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NObiective.Count(ob => ob.IdFirma == idFirmaFilter && ob.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var obiectiveQuery = from nOb in _epersContext.NObiective
                                         join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                         join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                         from post in postJoin.DefaultIfEmpty()
                                         where nOb.IdFirma == idFirmaFilter && nOb.Denumire.Contains(filter)
                                         select new AfisareNObiective
                                         {
                                             Id = nOb.Id,
                                             IsFaraProcent = nOb.IsFaraProcent,
                                             ValMax = nOb.ValMax,
                                             ValMin = nOb.ValMin,
                                             ValTarget = nOb.ValTarget,
                                             BonusMax = nOb.BonusMax,
                                             BonusMin = nOb.BonusMin,
                                             BonusTarget = nOb.BonusTarget,
                                             Denumire = nOb.Denumire,
                                             Frecventa = nOb.Frecventa,
                                             IdFirma = nOb.IdFirma,
                                             IsBonusProcentual = nOb.IsBonusProcentual,
                                             Firma = firma.Denumire,
                                             Tip = nOb.Tip,
                                             Post = post.Nume
                                         };
                    afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NObiective.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var obiectiveQuery = from nOb in _epersContext.NObiective
                                         join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                         join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                         from post in postJoin.DefaultIfEmpty()
                                         select new AfisareNObiective
                                         {
                                             Id = nOb.Id,
                                             IsFaraProcent = nOb.IsFaraProcent,
                                             ValMax = nOb.ValMax,
                                             ValMin = nOb.ValMin,
                                             ValTarget = nOb.ValTarget,
                                             BonusMax = nOb.BonusMax,
                                             BonusMin = nOb.BonusMin,
                                             BonusTarget = nOb.BonusTarget,
                                             Denumire = nOb.Denumire,
                                             Frecventa = nOb.Frecventa,
                                             IdFirma = nOb.IdFirma,
                                             IsBonusProcentual = nOb.IsBonusProcentual,
                                             Firma = firma.Denumire,
                                             Tip = nOb.Tip,
                                             Post = post.Nume
                                         };
                    afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NObiective.Count(ob => ob.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var obiectiveQuery = from nOb in _epersContext.NObiective
                                         join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                         join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                         from post in postJoin.DefaultIfEmpty()
                                         where nOb.Denumire.Contains(filter)
                                         select new AfisareNObiective
                                         {
                                             Id = nOb.Id,
                                             IsFaraProcent = nOb.IsFaraProcent,
                                             ValMax = nOb.ValMax,
                                             ValMin = nOb.ValMin,
                                             ValTarget = nOb.ValTarget,
                                             BonusMax = nOb.BonusMax,
                                             BonusMin = nOb.BonusMin,
                                             BonusTarget = nOb.BonusTarget,
                                             Denumire = nOb.Denumire,
                                             Frecventa = nOb.Frecventa,
                                             IdFirma = nOb.IdFirma,
                                             IsBonusProcentual = nOb.IsBonusProcentual,
                                             Firma = firma.Denumire,
                                             Tip = nOb.Tip,
                                             Post = post.Nume
                                         };
                    afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new AfisareNObiectiveDisplayModel
            {
                AfisNomObiectiveData = afisareNObiective,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };

        }

        public AfisareNObiectiveDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var afisareNObiective = Array.Empty<AfisareNObiective>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();
            var selBoxTipuriOb = _efSelectionBoxRepository.GetTipuriObiective();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NObiective.Count(ob => ob.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var obiectiveQuery = from nOb in _epersContext.NObiective
                                     join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                     from firma in fromaJoin.DefaultIfEmpty()
                                     join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                     from post in postJoin.DefaultIfEmpty()
                                     where nOb.IdFirma == loggedInUserFirma
                                     select new AfisareNObiective
                                     {
                                         Id = nOb.Id,
                                         IsFaraProcent = nOb.IsFaraProcent,
                                         ValMax = nOb.ValMax,
                                         ValMin = nOb.ValMin,
                                         ValTarget = nOb.ValTarget,
                                         BonusMax = nOb.BonusMax,
                                         BonusMin = nOb.BonusMin,
                                         BonusTarget = nOb.BonusTarget,
                                         Denumire = nOb.Denumire,
                                         Frecventa = nOb.Frecventa,
                                         IdFirma = nOb.IdFirma,
                                         IsBonusProcentual = nOb.IsBonusProcentual,
                                         Firma = firma.Denumire,
                                         Tip = nOb.Tip,
                                         Post = post.Nume
                                     };
                afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NObiective.Count(ob => ob.IdFirma == loggedInUserFirma && ob.Denumire.Contains(filter));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var obiectiveQuery = from nOb in _epersContext.NObiective
                                     join firma in _epersContext.NFirme on nOb.IdFirma equals firma.Id into fromaJoin
                                     from firma in fromaJoin.DefaultIfEmpty()
                                     join post in _epersContext.NPosturi on nOb.IdPost equals post.Id into postJoin
                                     from post in postJoin.DefaultIfEmpty()
                                     where nOb.IdFirma == loggedInUserFirma && nOb.Denumire.Contains(filter)
                                     select new AfisareNObiective
                                     {
                                         Id = nOb.Id,
                                         IsFaraProcent = nOb.IsFaraProcent,
                                         ValMax = nOb.ValMax,
                                         ValMin = nOb.ValMin,
                                         ValTarget = nOb.ValTarget,
                                         BonusMax = nOb.BonusMax,
                                         BonusMin = nOb.BonusMin,
                                         BonusTarget = nOb.BonusTarget,
                                         Denumire = nOb.Denumire,
                                         Frecventa = nOb.Frecventa,
                                         IdFirma = nOb.IdFirma,
                                         IsBonusProcentual = nOb.IsBonusProcentual,
                                         Firma = firma.Denumire,
                                         Tip = nOb.Tip,
                                         Post = post.Nume
                                     };
                afisareNObiective = obiectiveQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new AfisareNObiectiveDisplayModel
            {
                AfisNomObiectiveData = afisareNObiective,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };

        }

        private void CalculBonusProcentual(NObiective obiectiv)
        {
            // presupunem ca un bonus de 10 RON ii prea mic, dar un procent de 10x ii prea mare
            if (obiectiv.BonusMin >= 10)
                obiectiv.BonusMin = obiectiv.BonusMin / 100;
            if (obiectiv.BonusTarget >= 10)
                obiectiv.BonusTarget = obiectiv.BonusTarget / 100;
            if (obiectiv.BonusMax >= 10)
                obiectiv.BonusMax = obiectiv.BonusMax / 100;
        }

    }
}
