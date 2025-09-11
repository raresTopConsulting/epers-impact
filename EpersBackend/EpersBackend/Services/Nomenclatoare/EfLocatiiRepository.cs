using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfLocatiiRepository : IEfLocatiiRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;
        private readonly ILogger<NLocatii> _logger;

        public EfLocatiiRepository(EpersContext epersContext,
            IPagination paginationService,
            ILogger<NLocatii> logger)
        {
            _epersContext = epersContext;
            _paginationService = paginationService;
            _logger = logger;
        }

        public void Add(NLocatii locatie)
        {
            SetSediuPrincipalFirmaOnFalse(locatie);

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NLocatii.Add(locatie);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Locatii: Add - Locatie");
                throw;
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var foundLoc = _epersContext.NLocatii.Single(lc => lc.Id == id);
                foundLoc.DataSf = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NLocatii.Update(foundLoc);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Locatii: SetToInactive - Locatie");
                throw;
            }
        }

        public NLocatii[] GetAll()
        {
            var locatii = _epersContext.NLocatii.Select(lc => lc).ToArray();

            foreach (var item in locatii)
            {
                if (item.DataSf.HasValue)
                    item.Activ = DateTime.Compare(item.DataSf.Value, DateTime.Now) > 0 ? true : false;
                else if (!item.DataSf.HasValue)
                    item.Activ = true;
            }

            return locatii;
        }

        public NLocatii Get(int id)
        {
            try
            {
                var locatie = _epersContext.NLocatii.Single(lc => lc.Id == id);

                if (locatie.IdFirma != null)
                {
                    var localitateSiFirma = _epersContext.NFirme.Where(frm => frm.Id == locatie.IdFirma)
                            .Select(frm => new NLocatii
                            {
                                Id = locatie.Id,
                                Denumire = locatie.Denumire,
                                DataIn = locatie.DataIn,
                                DataSf = locatie.DataSf,
                                Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                Adresa = locatie.Adresa,
                                Localitate = locatie.Localitate,
                                Judet = locatie.Judet,
                                Tara = locatie.Tara,
                                IdFirma = locatie.IdFirma,
                                Firma = frm.Denumire,
                                IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                            });

                    return localitateSiFirma.First();
                }
                return locatie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Locatii: Get - Locatie");
                throw;
            }
        }

        public LocatiiDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null)
        {
            var locatii = Array.Empty<NLocatii>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NLocatii.Count(lc => lc.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var locatiiFirme = from locatie in _epersContext.NLocatii
                                       join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where locatie.IdFirma == idFirmaFilter
                                       select new NLocatii
                                       {
                                           Id = locatie.Id,
                                           Denumire = locatie.Denumire,
                                           DataIn = locatie.DataIn,
                                           DataSf = locatie.DataSf,
                                           Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                           Adresa = locatie.Adresa,
                                           Localitate = locatie.Localitate,
                                           Judet = locatie.Judet,
                                           Tara = locatie.Tara,
                                           IdFirma = locatie.IdFirma,
                                           Firma = firma.Denumire,
                                           IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                       };
                    locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }

                else
                {
                    totalRows = _epersContext.NLocatii.Count(lc => lc.IdFirma == idFirmaFilter &&
                                 (lc.Denumire.Contains(filter)
                                || (lc.Adresa != null && lc.Adresa.Contains(filter))
                                || (lc.Localitate != null && lc.Localitate.Contains(filter))
                                || (lc.Tara != null && lc.Tara.Contains(filter))));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var locatiiFirme = from locatie in _epersContext.NLocatii
                                       join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where locatie.IdFirma == idFirmaFilter &&
                                       (locatie.Denumire.Contains(filter)
                                        || (locatie.Adresa != null && locatie.Adresa.Contains(filter))
                                        || (locatie.Localitate != null && locatie.Localitate.Contains(filter))
                                        || (locatie.Tara != null && locatie.Tara.Contains(filter)))

                                       select new NLocatii
                                       {
                                           Id = locatie.Id,
                                           Denumire = locatie.Denumire,
                                           DataIn = locatie.DataIn,
                                           DataSf = locatie.DataSf,
                                           Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                           Adresa = locatie.Adresa,
                                           Localitate = locatie.Localitate,
                                           Judet = locatie.Judet,
                                           Tara = locatie.Tara,
                                           IdFirma = locatie.IdFirma,
                                           Firma = firma.Denumire,
                                           IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                       };
                    locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            else
            {
                totalRows = string.IsNullOrWhiteSpace(filter) ? _epersContext.NLocatii.Count() :
                        _epersContext.NLocatii.Count(lc => lc.Denumire.Contains(filter)
                        || (lc.Adresa != null && lc.Adresa.Contains(filter))
                        || (lc.Localitate != null && lc.Localitate.Contains(filter))
                        || (lc.Tara != null && lc.Tara.Contains(filter)));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                if (string.IsNullOrWhiteSpace(filter))
                {
                    var locatiiFirme = from locatie in _epersContext.NLocatii
                                       join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       select new NLocatii
                                       {
                                           Id = locatie.Id,
                                           Denumire = locatie.Denumire,
                                           DataIn = locatie.DataIn,
                                           DataSf = locatie.DataSf,
                                           Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                           Adresa = locatie.Adresa,
                                           Localitate = locatie.Localitate,
                                           Judet = locatie.Judet,
                                           Tara = locatie.Tara,
                                           IdFirma = locatie.IdFirma,
                                           Firma = firma.Denumire,
                                           IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                       };
                    locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    var locatiiFirme = from locatie in _epersContext.NLocatii
                                       join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where locatie.Denumire.Contains(filter)
                                        || (locatie.Adresa != null && locatie.Adresa.Contains(filter))
                                        || (locatie.Localitate != null && locatie.Localitate.Contains(filter))
                                        || (locatie.Tara != null && locatie.Tara.Contains(filter))

                                       select new NLocatii
                                       {
                                           Id = locatie.Id,
                                           Denumire = locatie.Denumire,
                                           DataIn = locatie.DataIn,
                                           DataSf = locatie.DataSf,
                                           Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                           Adresa = locatie.Adresa,
                                           Localitate = locatie.Localitate,
                                           Judet = locatie.Judet,
                                           Tara = locatie.Tara,
                                           IdFirma = locatie.IdFirma,
                                           Firma = firma.Denumire,
                                           IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                       };
                    locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new LocatiiDisplayModel
            {
                Locatii = locatii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        LocatiiDisplayModel IEfLocatiiRepository.GetAllPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma, string? filter)
        {
            var locatii = Array.Empty<NLocatii>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NLocatii.Count(lc => lc.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var locatiiFirme = from locatie in _epersContext.NLocatii
                                   join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                   from firma in fromaJoin.DefaultIfEmpty()
                                   where locatie.IdFirma == loggedInUserFirma
                                   select new NLocatii
                                   {
                                       Id = locatie.Id,
                                       Denumire = locatie.Denumire,
                                       DataIn = locatie.DataIn,
                                       DataSf = locatie.DataSf,
                                       Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                       Adresa = locatie.Adresa,
                                       Localitate = locatie.Localitate,
                                       Judet = locatie.Judet,
                                       Tara = locatie.Tara,
                                       IdFirma = locatie.IdFirma,
                                       Firma = firma.Denumire,
                                       IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                   };
                locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NLocatii.Count(lc => lc.IdFirma == loggedInUserFirma &&
                             (lc.Denumire.Contains(filter)
                            || (lc.Adresa != null && lc.Adresa.Contains(filter))
                            || (lc.Localitate != null && lc.Localitate.Contains(filter))
                            || (lc.Tara != null && lc.Tara.Contains(filter))));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var locatiiFirme = from locatie in _epersContext.NLocatii
                                   join firma in _epersContext.NFirme on locatie.IdFirma equals firma.Id into fromaJoin
                                   from firma in fromaJoin.DefaultIfEmpty()
                                   where locatie.IdFirma == loggedInUserFirma &&
                                   (locatie.Denumire.Contains(filter)
                                    || (locatie.Adresa != null && locatie.Adresa.Contains(filter))
                                    || (locatie.Localitate != null && locatie.Localitate.Contains(filter))
                                    || (locatie.Tara != null && locatie.Tara.Contains(filter)))

                                   select new NLocatii
                                   {
                                       Id = locatie.Id,
                                       Denumire = locatie.Denumire,
                                       DataIn = locatie.DataIn,
                                       DataSf = locatie.DataSf,
                                       Activ = locatie.DataSf.HasValue && DateTime.Compare(locatie.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                       Adresa = locatie.Adresa,
                                       Localitate = locatie.Localitate,
                                       Judet = locatie.Judet,
                                       Tara = locatie.Tara,
                                       IdFirma = locatie.IdFirma,
                                       Firma = firma.Denumire,
                                       IsSediuPrincipalFirma = locatie.IsSediuPrincipalFirma
                                   };
                locatii = locatiiFirme.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new LocatiiDisplayModel
            {
                Locatii = locatii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public void Update(NLocatii locatie)
        {
            SetSediuPrincipalFirmaOnFalse(locatie);

            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NLocatii.Update(locatie);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Locatii: Update - Locatie");
                throw;
            }
        }

        private void SetSediuPrincipalFirmaOnFalse(NLocatii locatie)
        {
            if (locatie.IsSediuPrincipalFirma == true)
            {
                try
                {
                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.NLocatii
                        .Where(lc => lc.IdFirma == locatie.IdFirma && lc.IsSediuPrincipalFirma == true)
                        .ToList()
                        .ForEach(row => row.IsSediuPrincipalFirma = false);

                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Locatii: SetSediuPrincipalOnFase");
                    throw;
                }
            }
        }

    }
}

