using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfPosturiRepository : IEfPosturiRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;
        private readonly ILogger<NPosturi> _logger;

        public EfPosturiRepository(EpersContext epersContext,
            IPagination paginationService,
            ILogger<NPosturi> logger)
        {
            _epersContext = epersContext;
            _paginationService = paginationService;
            _logger = logger;
        }

        public void Add(NPosturi post)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NPosturi.Add(post);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPosturi: Add - Post");
                throw;
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var found = _epersContext.NPosturi.Single(lc => lc.Id == id);
                found.DataSf = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NPosturi.Update(found);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPosturi: SetToInactive - Post");
                throw;
            }
        }

        public NPosturi Get(int id)
        {
            try
            {
                return _epersContext.NPosturi.Single(lc => lc.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPosturi: Get - Post");
                throw;
            }
        }

        public PosturiDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var posturi = Array.Empty<NPosturi>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NPosturi.Count(pst => pst.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var postQuery = from post in _epersContext.NPosturi
                                join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                from firma in fromaJoin.DefaultIfEmpty()
                                where post.IdFirma == loggedInUserFirma
                                select new NPosturi
                                {
                                    Id = post.Id,
                                    Nume = post.Nume,
                                    IdFirma = post.IdFirma,
                                    Firma = firma.Denumire,
                                    DataIn = post.DataIn,
                                    DataSf = post.DataSf,
                                    DenFunctie = post.DenFunctie,
                                    ProfilCompetente = post.ProfilCompetente,
                                    COR = post.COR,
                                    NivelPost = post.NivelPost,
                                    Punctaj = post.Punctaj,
                                    Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                };
                posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NPosturi.Count(pst => pst.IdFirma == loggedInUserFirma && (pst.Nume.Contains(filter)
                            || (pst.DenFunctie != null && pst.DenFunctie.Contains(filter))));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var postQuery = from post in _epersContext.NPosturi
                                join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                from firma in fromaJoin.DefaultIfEmpty()
                                where post.IdFirma == loggedInUserFirma
                                && (post.Nume.Contains(filter) || (post.DenFunctie != null && post.DenFunctie.Contains(filter)))
                                select new NPosturi
                                {
                                    Id = post.Id,
                                    Nume = post.Nume,
                                    IdFirma = post.IdFirma,
                                    Firma = firma.Denumire,
                                    DataIn = post.DataIn,
                                    DataSf = post.DataSf,
                                    DenFunctie = post.DenFunctie,
                                    ProfilCompetente = post.ProfilCompetente,
                                    COR = post.COR,
                                    NivelPost = post.NivelPost,
                                    Punctaj = post.Punctaj,
                                    Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                };
                posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new PosturiDisplayModel
            {
                Posturi = posturi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };

            
        }

        public void Update(NPosturi post)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NPosturi.Update(post);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPosturi: Update - Post");
                throw;
            }
        }

        public PosturiDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var posturi = Array.Empty<NPosturi>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NPosturi.Count(pst => pst.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var postQuery = from post in _epersContext.NPosturi
                                    join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where post.IdFirma == idFirmaFilter
                                    select new NPosturi
                                    {
                                        Id = post.Id,
                                        Nume = post.Nume,
                                        IdFirma = post.IdFirma,
                                        Firma = firma.Denumire,
                                        DataIn = post.DataIn,
                                        DataSf = post.DataSf,
                                        DenFunctie = post.DenFunctie,
                                        ProfilCompetente = post.ProfilCompetente,
                                        COR = post.COR,
                                        NivelPost = post.NivelPost,
                                        Punctaj = post.Punctaj,
                                        Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                    };
                    posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NPosturi.Count(pst => pst.IdFirma == idFirmaFilter && (pst.Nume.Contains(filter)
                                || (pst.DenFunctie != null && pst.DenFunctie.Contains(filter))));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var postQuery = from post in _epersContext.NPosturi
                                    join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where post.IdFirma == idFirmaFilter
                                    && (post.Nume.Contains(filter) || (post.DenFunctie != null && post.DenFunctie.Contains(filter)))
                                    select new NPosturi
                                    {
                                        Id = post.Id,
                                        Nume = post.Nume,
                                        IdFirma = post.IdFirma,
                                        Firma = firma.Denumire,
                                        DataIn = post.DataIn,
                                        DataSf = post.DataSf,
                                        DenFunctie = post.DenFunctie,
                                        ProfilCompetente = post.ProfilCompetente,
                                        COR = post.COR,
                                        NivelPost = post.NivelPost,
                                        Punctaj = post.Punctaj,
                                        Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                    };
                    posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NPosturi.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var postQuery = from post in _epersContext.NPosturi
                                    join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    select new NPosturi
                                    {
                                        Id = post.Id,
                                        Nume = post.Nume,
                                        IdFirma = post.IdFirma,
                                        Firma = firma.Denumire,
                                        DataIn = post.DataIn,
                                        DataSf = post.DataSf,
                                        DenFunctie = post.DenFunctie,
                                        ProfilCompetente = post.ProfilCompetente,
                                        COR = post.COR,
                                        NivelPost = post.NivelPost,
                                        Punctaj = post.Punctaj,
                                        Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                    };
                    posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NPosturi.Count(pst => pst.Nume.Contains(filter)
                                || (pst.DenFunctie != null && pst.DenFunctie.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var postQuery = from post in _epersContext.NPosturi
                                    join firma in _epersContext.NFirme on post.IdFirma equals firma.Id into fromaJoin
                                    from firma in fromaJoin.DefaultIfEmpty()
                                    where post.Nume.Contains(filter) || (post.DenFunctie != null && post.DenFunctie.Contains(filter))
                                    select new NPosturi
                                    {
                                        Id = post.Id,
                                        Nume = post.Nume,
                                        IdFirma = post.IdFirma,
                                        Firma = firma.Denumire,
                                        DataIn = post.DataIn,
                                        DataSf = post.DataSf,
                                        DenFunctie = post.DenFunctie,
                                        ProfilCompetente = post.ProfilCompetente,
                                        COR = post.COR,
                                        NivelPost = post.NivelPost,
                                        Punctaj = post.Punctaj,
                                        Activ = post.DataSf.HasValue && DateTime.Compare(post.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                    };
                    posturi = postQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new PosturiDisplayModel
            {
                Posturi = posturi,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }
    }
}

