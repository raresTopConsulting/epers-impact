using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfCursuriRepository : IEfCursuriRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;
        private readonly ILogger<NCursuri> _logger;

        public EfCursuriRepository(EpersContext epersContext, IPagination paginationService, ILogger<NCursuri> logger)
        {
            _epersContext = epersContext;
            _paginationService = paginationService;
            _logger = logger;
        }

        public void Add(NCursuri curs)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCursuri.Add(curs);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCursuriRepository: Add");
                throw;
            }
        }

        public NCursuri Get(int id)
        {
            try
            {
                return _epersContext.NCursuri.Single(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCursuriRepository: Get");
                throw;
            }
        }

        public CursuriDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null)
        {
            var cursuri = Array.Empty<NCursuri>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            totalRows = string.IsNullOrWhiteSpace(filter) ? _epersContext.NCursuri.Count() :
                _epersContext.NCursuri.Count(c => c.Denumire.Contains(filter));

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NCursuri.Count(crs => crs.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var cursQuery = from curs in _epersContext.NCursuri
                                    join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where curs.IdFirma == idFirmaFilter
                                    select new NCursuri
                                    {
                                        Id = curs.Id,
                                        Denumire = curs.Denumire,
                                        IdFirma = curs.IdFirma,
                                        Firma = firma.Denumire,
                                        Organizator = curs.Organizator,
                                        Pret = curs.Pret,
                                        DataInceput = curs.DataInceput,
                                        DataSfarsit = curs.DataSfarsit,
                                        Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                        Locatie = curs.Locatie,
                                        IsOnline = curs.IsOnline,
                                        Link = curs.Link
                                    };
                    cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NCursuri.Count(crs => crs.IdFirma == idFirmaFilter && crs.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var cursQuery = from curs in _epersContext.NCursuri
                                    join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where curs.IdFirma == idFirmaFilter && curs.Denumire.Contains(filter)
                                    select new NCursuri
                                    {
                                        Id = curs.Id,
                                        Denumire = curs.Denumire,
                                        IdFirma = curs.IdFirma,
                                        Firma = firma.Denumire,
                                        Organizator = curs.Organizator,
                                        Pret = curs.Pret,
                                        DataInceput = curs.DataInceput,
                                        DataSfarsit = curs.DataSfarsit,
                                        Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                        Locatie = curs.Locatie,
                                        IsOnline = curs.IsOnline,
                                        Link = curs.Link
                                    };
                    cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NCursuri.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var cursQuery = from curs in _epersContext.NCursuri
                                    join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    select new NCursuri
                                    {
                                        Id = curs.Id,
                                        Denumire = curs.Denumire,
                                        IdFirma = curs.IdFirma,
                                        Firma = firma.Denumire,
                                        Organizator = curs.Organizator,
                                        Pret = curs.Pret,
                                        DataInceput = curs.DataInceput,
                                        DataSfarsit = curs.DataSfarsit,
                                        Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                        Locatie = curs.Locatie,
                                        IsOnline = curs.IsOnline,
                                        Link = curs.Link
                                    };
                    cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NCursuri.Count(crs => crs.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var cursQuery = from curs in _epersContext.NCursuri
                                    join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where curs.Denumire.Contains(filter)
                                    select new NCursuri
                                    {
                                        Id = curs.Id,
                                        Denumire = curs.Denumire,
                                        IdFirma = curs.IdFirma,
                                        Firma = firma.Denumire,
                                        Organizator = curs.Organizator,
                                        Pret = curs.Pret,
                                        DataInceput = curs.DataInceput,
                                        DataSfarsit = curs.DataSfarsit,
                                        Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                        Locatie = curs.Locatie,
                                        IsOnline = curs.IsOnline,
                                        Link = curs.Link
                                    };
                    cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new CursuriDisplayModel
            {
                Cursuri = cursuri,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };

        }

        public CursuriDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var cursuri = Array.Empty<NCursuri>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            totalRows = string.IsNullOrWhiteSpace(filter) ? _epersContext.NCursuri.Count() :
                _epersContext.NCursuri.Count(c => c.Denumire.Contains(filter));

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NCursuri.Count(crs => crs.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var cursQuery = from curs in _epersContext.NCursuri
                                join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                from firma in fromaJoin.DefaultIfEmpty()
                                where curs.IdFirma == loggedInUserFirma
                                select new NCursuri
                                {
                                    Id = curs.Id,
                                    Denumire = curs.Denumire,
                                    IdFirma = curs.IdFirma,
                                    Firma = firma.Denumire,
                                    Organizator = curs.Organizator,
                                    Pret = curs.Pret,
                                    DataInceput = curs.DataInceput,
                                    DataSfarsit = curs.DataSfarsit,
                                    Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                    Locatie = curs.Locatie,
                                    IsOnline = curs.IsOnline,
                                    Link = curs.Link
                                };
                cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NCursuri.Count(crs => crs.IdFirma == loggedInUserFirma && crs.Denumire.Contains(filter));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var cursQuery = from curs in _epersContext.NCursuri
                                join firma in _epersContext.NFirme on curs.IdFirma equals firma.Id into fromaJoin
                                from firma in fromaJoin.DefaultIfEmpty()
                                where curs.IdFirma == loggedInUserFirma && curs.Denumire.Contains(filter)
                                select new NCursuri
                                {
                                    Id = curs.Id,
                                    Denumire = curs.Denumire,
                                    IdFirma = curs.IdFirma,
                                    Firma = firma.Denumire,
                                    Organizator = curs.Organizator,
                                    Pret = curs.Pret,
                                    DataInceput = curs.DataInceput,
                                    DataSfarsit = curs.DataSfarsit,
                                    Activ = curs.DataSfarsit.HasValue && DateTime.Compare(curs.DataSfarsit.Value, DateTime.Now) <= 0 ? false : true,
                                    Locatie = curs.Locatie,
                                    IsOnline = curs.IsOnline,
                                    Link = curs.Link
                                };
                cursuri = cursQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new CursuriDisplayModel
            {
                Cursuri = cursuri,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };

        }

        public void SetToInactive(int id)
        {
            try
            {
                var found = _epersContext.NCursuri.Single(c => c.Id == id);
                found.DataSfarsit = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCursuri.Update(found);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "EfCursuriRepository: Delete");
                throw;
            }
        }

        public void Update(NCursuri curs)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCursuri.Update(curs);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCursuriRepository: Update");
                throw;
            }
        }
    }
}
