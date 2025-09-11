using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfCompartimenteRepository : IEfCompartimenteRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;
        private readonly ILogger<NCompartimente> _logger;

        public EfCompartimenteRepository(EpersContext epersContext,
            ILogger<NCompartimente> logger,
            IPagination paginationService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _paginationService = paginationService;
        }

        public void Add(NCompartimente compartiment)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCompartimente.Add(compartiment);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCompoartimenteRepository: Add");
                throw;
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var found = _epersContext.NCompartimente.Single(lc => lc.Id == id);
                found.Data_sf = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCompartimente.Update(found);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "EfCompoartimenteRepository: Delete");
                throw;
            }
        }

        public NCompartimente Get(int id)
        {
            try
            {
                return _epersContext.NCompartimente.Single(cmp => cmp.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCompoartimenteRepository: Get");
                throw;
            }
        }

        public CompartimenteDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null)
        {
            var compartimente = Array.Empty<NCompartimentDisplay>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NCompartimente.Count(cmp => cmp.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                             join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                             from firma in fromaJoin.DefaultIfEmpty()
                                             join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                             from nLocatii in locatieJoin.DefaultIfEmpty()
                                             where compartiment.IdFirma == idFirmaFilter
                                             select new NCompartimentDisplay
                                             {
                                                 Id = compartiment.Id,
                                                 Id_Locatie = compartiment.Id_Locatie,
                                                 Data_in = compartiment.Data_in,
                                                 Data_sf = compartiment.Data_sf,
                                                 Activ = compartiment.Data_sf.HasValue
                                                    ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                    ? true : false) : true,
                                                 Denumire = compartiment.Denumire,
                                                 Locatie = nLocatii.Denumire,
                                                 Jos = compartiment.Jos,
                                                 SubCompartiment = compartiment.SubCompartiment,
                                                 Sus = compartiment.Sus,
                                                 Firma = firma.Denumire,
                                                 IdFirma = compartiment.IdFirma
                                             };
                    compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NCompartimente.Count(cmp => cmp.IdFirma == idFirmaFilter && cmp.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                             join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                             from firma in fromaJoin.DefaultIfEmpty()
                                            join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                             from nLocatii in locatieJoin.DefaultIfEmpty()
                                             where compartiment.IdFirma == idFirmaFilter && compartiment.Denumire.Contains(filter)
                                             select new NCompartimentDisplay
                                             {
                                                 Id = compartiment.Id,
                                                 Id_Locatie = compartiment.Id_Locatie,
                                                 Data_in = compartiment.Data_in,
                                                 Data_sf = compartiment.Data_sf,
                                                 Activ = compartiment.Data_sf.HasValue
                                                    ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                    ? true : false) : true,
                                                 Denumire = compartiment.Denumire,
                                                 Locatie = nLocatii.Denumire,
                                                 Jos = compartiment.Jos,
                                                 SubCompartiment = compartiment.SubCompartiment,
                                                 Sus = compartiment.Sus,
                                                 Firma = firma.Denumire,
                                                 IdFirma = compartiment.IdFirma
                                             };
                    compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NCompartimente.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                             join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                             from firma in fromaJoin.DefaultIfEmpty()
                                             join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                             from nLocatii in locatieJoin.DefaultIfEmpty()
                                             select new NCompartimentDisplay
                                             {
                                                 Id = compartiment.Id,
                                                 Id_Locatie = compartiment.Id_Locatie,
                                                 Data_in = compartiment.Data_in,
                                                 Data_sf = compartiment.Data_sf,
                                                 Activ = compartiment.Data_sf.HasValue
                                                    ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                    ? true : false) : true,
                                                 Denumire = compartiment.Denumire,
                                                 Locatie = nLocatii.Denumire,
                                                 Jos = compartiment.Jos,
                                                 SubCompartiment = compartiment.SubCompartiment,
                                                 Sus = compartiment.Sus,
                                                 Firma = firma.Denumire,
                                                 IdFirma = compartiment.IdFirma
                                             };
                    compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NCompartimente.Count(cmp => cmp.Denumire.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                             join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                             from firma in fromaJoin.DefaultIfEmpty()
                                            join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                             from nLocatii in locatieJoin.DefaultIfEmpty()
                                             where compartiment.Denumire.Contains(filter)
                                             select new NCompartimentDisplay
                                             {
                                                 Id = compartiment.Id,
                                                 Id_Locatie = compartiment.Id_Locatie,
                                                 Data_in = compartiment.Data_in,
                                                 Data_sf = compartiment.Data_sf,
                                                 Activ = compartiment.Data_sf.HasValue
                                                    ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                    ? true : false) : true,
                                                 Denumire = compartiment.Denumire,
                                                 Locatie = nLocatii.Denumire,
                                                 Jos = compartiment.Jos,
                                                 SubCompartiment = compartiment.SubCompartiment,
                                                 Sus = compartiment.Sus,
                                                 Firma = firma.Denumire,
                                                 IdFirma = compartiment.IdFirma
                                             };
                    compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new CompartimenteDisplayModel
            {
                Compartimente = compartimente,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };

        }

        public CompartimenteDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var compartimente = Array.Empty<NCompartimentDisplay>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NCompartimente.Count(cmp => cmp.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                         join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                        join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                         from nLocatii in locatieJoin.DefaultIfEmpty()
                                         where compartiment.IdFirma == loggedInUserFirma
                                         select new NCompartimentDisplay
                                         {
                                             Id = compartiment.Id,
                                             Id_Locatie = compartiment.Id_Locatie,
                                             Data_in = compartiment.Data_in,
                                             Data_sf = compartiment.Data_sf,
                                             Activ = compartiment.Data_sf.HasValue
                                                ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                ? true : false) : true,
                                             Denumire = compartiment.Denumire,
                                             Locatie = nLocatii.Denumire,
                                             Jos = compartiment.Jos,
                                             SubCompartiment = compartiment.SubCompartiment,
                                             Sus = compartiment.Sus,
                                             Firma = firma.Denumire,
                                             IdFirma = compartiment.IdFirma
                                         };
                compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NCompartimente.Count(cmp => cmp.IdFirma == loggedInUserFirma && cmp.Denumire.Contains(filter));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var compartimenteQuery = from compartiment in _epersContext.NCompartimente
                                         join firma in _epersContext.NFirme on compartiment.IdFirma equals firma.Id into fromaJoin
                                         from firma in fromaJoin.DefaultIfEmpty()
                                         join nLocatii in _epersContext.NLocatii on compartiment.Id_Locatie equals nLocatii.Id into locatieJoin
                                         from nLocatii in locatieJoin.DefaultIfEmpty()
                                         where compartiment.IdFirma == loggedInUserFirma && compartiment.Denumire.Contains(filter)
                                         select new NCompartimentDisplay
                                         {
                                             Id = compartiment.Id,
                                             Id_Locatie = compartiment.Id_Locatie,
                                             Data_in = compartiment.Data_in,
                                             Data_sf = compartiment.Data_sf,
                                             Activ = compartiment.Data_sf.HasValue
                                                ? (DateTime.Compare(compartiment.Data_sf.Value, DateTime.Now) > 0
                                                ? true : false) : true,
                                             Denumire = compartiment.Denumire,
                                             Locatie = nLocatii.Denumire,
                                             Jos = compartiment.Jos,
                                             SubCompartiment = compartiment.SubCompartiment,
                                             Sus = compartiment.Sus,
                                             Firma = firma.Denumire,
                                             IdFirma = compartiment.IdFirma
                                         };
                compartimente = compartimenteQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new CompartimenteDisplayModel
            {
                Compartimente = compartimente,
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };
        }

        public void Update(NCompartimente compartiement)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NCompartimente.Update(compartiement);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfCompoartimenteRepository: Update");
                throw;
            }
        }
    }
}

