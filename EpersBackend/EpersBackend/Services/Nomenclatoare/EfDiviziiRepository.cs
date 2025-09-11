using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfDiviziiRepository : IEfDiviziiRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;

        public EfDiviziiRepository(EpersContext epersContext,
            IPagination paginationService)
        {
            _epersContext = epersContext;
            _paginationService = paginationService;
        }

        public void Add(NDivizii divizie)
        {
            using (var dbTransaction = _epersContext.Database.BeginTransaction())
            {
                _epersContext.NDivizii.Add(divizie);
                _epersContext.SaveChanges();
                dbTransaction.Commit();
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var found = _epersContext.NDivizii.SingleOrDefault(lc => lc.Id == id);
                if (found != null)
                {
                    found.DataSf = DateTime.Now;

                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.NDivizii.Update(found);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
        }

        public NDivizii Get(int id)
        {
            try
            {
                return _epersContext.NDivizii.Single(lc => lc.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return ex.InnerException != null ?
                    throw new Exception(ex.InnerException.Message)
                    : throw new Exception(ex.Message); ;
            }
        }

        public DiviziiDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
          string? filter = null, int? idFirmaFilter = null)
        {
            var divizii = Array.Empty<NDivizii>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NDivizii.Count(dv => dv.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var diviziiQuery = from divizie in _epersContext.NDivizii
                                       join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where divizie.IdFirma == idFirmaFilter
                                       select new NDivizii
                                       {
                                           Id = divizie.Id,
                                           IdFirma = divizie.IdFirma,
                                           Firma = firma.Denumire,
                                           Denumire = divizie.Denumire,
                                           Descriere = divizie.Descriere,
                                           DataIn = divizie.DataIn,
                                           DataSf = divizie.DataSf,
                                           Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                       };
                    divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NDivizii.Count(dv => dv.IdFirma == idFirmaFilter
                        && ((dv.Denumire != null && dv.Denumire.Contains(filter)) || (dv.Descriere != null && dv.Descriere.Contains(filter))));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var diviziiQuery = from divizie in _epersContext.NDivizii
                                       join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where divizie.IdFirma == idFirmaFilter
                                        && ((divizie.Denumire != null && divizie.Denumire.Contains(filter)) || (divizie.Descriere != null && divizie.Descriere.Contains(filter)))
                                       select new NDivizii
                                       {
                                           Id = divizie.Id,
                                           IdFirma = divizie.IdFirma,
                                           Firma = firma.Denumire,
                                           Denumire = divizie.Denumire,
                                           Descriere = divizie.Descriere,
                                           DataIn = divizie.DataIn,
                                           DataSf = divizie.DataSf,
                                           Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                       };
                    divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NDivizii.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var diviziiQuery = from divizie in _epersContext.NDivizii
                                       join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       select new NDivizii
                                       {
                                           Id = divizie.Id,
                                           IdFirma = divizie.IdFirma,
                                           Firma = firma.Denumire,
                                           Denumire = divizie.Denumire,
                                           Descriere = divizie.Descriere,
                                           DataIn = divizie.DataIn,
                                           DataSf = divizie.DataSf,
                                           Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                       };
                    divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NDivizii.Count(dv => (dv.Denumire != null && dv.Denumire.Contains(filter)) || (dv.Descriere != null && dv.Descriere.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var diviziiQuery = from divizie in _epersContext.NDivizii
                                       join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                       from firma in fromaJoin.DefaultIfEmpty()
                                       where (divizie.Denumire != null && divizie.Denumire.Contains(filter)) || (divizie.Descriere != null && divizie.Descriere.Contains(filter))
                                       select new NDivizii
                                       {
                                           Id = divizie.Id,
                                           IdFirma = divizie.IdFirma,
                                           Firma = firma.Denumire,
                                           Denumire = divizie.Denumire,
                                           Descriere = divizie.Descriere,
                                           DataIn = divizie.DataIn,
                                           DataSf = divizie.DataSf,
                                           Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                       };
                    divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new DiviziiDisplayModel
            {
                Divizii = divizii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public DiviziiDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
                    int loggedInUserFirma, string? filter = null)
        {
            var divizii = Array.Empty<NDivizii>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NDivizii.Count(dv => dv.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var diviziiQuery = from divizie in _epersContext.NDivizii
                                   join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                   from firma in fromaJoin.DefaultIfEmpty()
                                   where divizie.IdFirma == loggedInUserFirma
                                   select new NDivizii
                                   {
                                       Id = divizie.Id,
                                       IdFirma = divizie.IdFirma,
                                       Firma = firma.Denumire,
                                       Denumire = divizie.Denumire,
                                       Descriere = divizie.Descriere,
                                       DataIn = divizie.DataIn,
                                       DataSf = divizie.DataSf,
                                       Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                   };
                divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NDivizii.Count(dv => dv.IdFirma == loggedInUserFirma
                    && ((dv.Denumire != null && dv.Denumire.Contains(filter)) || (dv.Descriere != null && dv.Descriere.Contains(filter))));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var diviziiQuery = from divizie in _epersContext.NDivizii
                                   join firma in _epersContext.NFirme on divizie.IdFirma equals firma.Id into fromaJoin
                                   from firma in fromaJoin.DefaultIfEmpty()
                                   where divizie.IdFirma == loggedInUserFirma
                                    && ((divizie.Denumire != null && divizie.Denumire.Contains(filter)) || (divizie.Descriere != null && divizie.Descriere.Contains(filter)))
                                   select new NDivizii
                                   {
                                       Id = divizie.Id,
                                       IdFirma = divizie.IdFirma,
                                       Firma = firma.Denumire,
                                       Denumire = divizie.Denumire,
                                       Descriere = divizie.Descriere,
                                       DataIn = divizie.DataIn,
                                       DataSf = divizie.DataSf,
                                       Activ = divizie.DataSf.HasValue && DateTime.Compare(divizie.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                   };
                divizii = diviziiQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new DiviziiDisplayModel
            {
                Divizii = divizii,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public void Update(NDivizii divizie)
        {
            using (var dbTransaction = _epersContext.Database.BeginTransaction())
            {
                _epersContext.NDivizii.Update(divizie);
                _epersContext.SaveChanges();
                dbTransaction.Commit();
            }
        }
    }
}


