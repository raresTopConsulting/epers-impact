using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using Epers.Models.Pagination;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfSkillsRepository : IEfSkillsRepository
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<NSkills> _logger;
        private readonly IPagination _paginationService;

        public EfSkillsRepository(EpersContext epersContext,
             ILogger<NSkills> logger, IPagination pagginationService)
        {
            _epersContext = epersContext;
            _logger = logger;
            _paginationService = pagginationService;
        }

        public void Add(NSkills skill)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NSkills.Add(skill);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfSkillsRepository: Add");
                throw;
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var found = _epersContext.NSkills.Single(lc => lc.Id == id);
                found.DataSf = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NSkills.Update(found);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "EfSkillsRepository: Delete");
                throw;
            }
        }

        public NSkills Get(int id)
        {
            try
            {
                return _epersContext.NSkills.Single(lc => lc.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfSkillsRepository: Get");
                throw;
            }
        }

        public SkillsDisplayModel GetAllForAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null)
        {
            var skills = Array.Empty<NSkills>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (idFirmaFilter.HasValue)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NSkills.Count(sk => sk.IdFirma == idFirmaFilter);
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var skillsQuery = from skill in _epersContext.NSkills
                                      join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                      from firma in fromaJoin.DefaultIfEmpty()
                                      where skill.IdFirma == idFirmaFilter
                                      select new NSkills
                                      {
                                          Id = skill.Id,
                                          Denumire = skill.Denumire,
                                          Descriere = skill.Descriere,
                                          IdFirma = skill.IdFirma,
                                          Firma = firma.Denumire,
                                          DataIn = skill.DataIn,
                                          DataSf = skill.DataSf,
                                          Tip = skill.Tip,
                                          Detalii = skill.Detalii,
                                          Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                      };
                    skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NSkills.Count(sk => sk.IdFirma == idFirmaFilter
                        && (sk.Denumire.Contains(filter) || sk.Descriere.Contains(filter)));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var skillsQuery = from skill in _epersContext.NSkills
                                      join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                      from firma in fromaJoin.DefaultIfEmpty()
                                      where skill.IdFirma == idFirmaFilter
                                      && (skill.Denumire.Contains(filter) || skill.Descriere.Contains(filter))
                                      select new NSkills
                                      {
                                          Id = skill.Id,
                                          Denumire = skill.Denumire,
                                          Descriere = skill.Descriere,
                                          IdFirma = skill.IdFirma,
                                          Firma = firma.Denumire,
                                          DataIn = skill.DataIn,
                                          DataSf = skill.DataSf,
                                          Tip = skill.Tip,
                                          Detalii = skill.Detalii,
                                          Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                      };
                    skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    totalRows = _epersContext.NSkills.Count();
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var skillsQuery = from skill in _epersContext.NSkills
                                      join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                      from firma in fromaJoin.DefaultIfEmpty()
                                      select new NSkills
                                      {
                                          Id = skill.Id,
                                          Denumire = skill.Denumire,
                                          Descriere = skill.Descriere,
                                          IdFirma = skill.IdFirma,
                                          Firma = firma.Denumire,
                                          DataIn = skill.DataIn,
                                          DataSf = skill.DataSf,
                                          Tip = skill.Tip,
                                          Detalii = skill.Detalii,
                                          Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true,

                                      };
                    skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
                else
                {
                    totalRows = _epersContext.NSkills.Count(sk => sk.Denumire.Contains(filter) || sk.Descriere.Contains(filter));
                    pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                    var skillsQuery = from skill in _epersContext.NSkills
                                      join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                      from firma in fromaJoin.DefaultIfEmpty()
                                      where skill.Denumire.Contains(filter) || skill.Descriere.Contains(filter)
                                      select new NSkills
                                      {
                                          Id = skill.Id,
                                          Denumire = skill.Denumire,
                                          Descriere = skill.Descriere,
                                          IdFirma = skill.IdFirma,
                                          Firma = firma.Denumire,
                                          DataIn = skill.DataIn,
                                          DataSf = skill.DataSf,
                                          Tip = skill.Tip,
                                          Detalii = skill.Detalii,
                                          Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                      };
                    skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
                }
            }

            return new SkillsDisplayModel
            {
                Skills = skills,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public SkillsDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            int loggedInUserFirma, string? filter = null)
        {
            var skills = Array.Empty<NSkills>();
            var totalRows = 0;
            var pageSettings = new PaginationModel();

            if (string.IsNullOrWhiteSpace(filter))
            {
                totalRows = _epersContext.NSkills.Count(sk => sk.IdFirma == loggedInUserFirma);
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var skillsQuery = from skill in _epersContext.NSkills
                                  join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                  from firma in fromaJoin.DefaultIfEmpty()
                                  where skill.IdFirma == loggedInUserFirma
                                  select new NSkills
                                  {
                                      Id = skill.Id,
                                      Denumire = skill.Denumire,
                                      Descriere = skill.Descriere,
                                      IdFirma = skill.IdFirma,
                                      Firma = firma.Denumire,
                                      DataIn = skill.DataIn,
                                      DataSf = skill.DataSf,
                                      Tip = skill.Tip,
                                      Detalii = skill.Detalii,
                                      Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                  };
                skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }
            else
            {
                totalRows = _epersContext.NSkills.Count(sk => sk.IdFirma == loggedInUserFirma
                    && (sk.Denumire.Contains(filter) || sk.Descriere.Contains(filter)));
                pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

                var skillsQuery = from skill in _epersContext.NSkills
                                  join firma in _epersContext.NFirme on skill.IdFirma equals firma.Id into fromaJoin
                                  from firma in fromaJoin.DefaultIfEmpty()
                                  where skill.IdFirma == loggedInUserFirma
                                  && (skill.Denumire.Contains(filter) || skill.Descriere.Contains(filter))
                                  select new NSkills
                                  {
                                      Id = skill.Id,
                                      Denumire = skill.Denumire,
                                      Descriere = skill.Descriere,
                                      IdFirma = skill.IdFirma,
                                      Firma = firma.Denumire,
                                      DataIn = skill.DataIn,
                                      DataSf = skill.DataSf,
                                      Tip = skill.Tip,
                                      Detalii = skill.Detalii,
                                      Activ = skill.DataSf.HasValue && DateTime.Compare(skill.DataSf.Value, DateTime.Now) <= 0 ? false : true
                                  };
                skills = skillsQuery.Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();
            }

            return new SkillsDisplayModel
            {
                Skills = skills,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }

        public void Update(NSkills skill)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NSkills.Update(skill);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfSkillsRepository: Update");
                throw;
            }
        }
    }
}

