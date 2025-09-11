using Epers.DataAccess;
using Epers.Models.Nomenclatoare;
using EpersBackend.Services.Pagination;

namespace EpersBackend.Services.Nomenclatoare
{
    public class EfFirmeRepository : IEfFirmeRepository
    {
        private readonly EpersContext _epersContext;
        private readonly IPagination _paginationService;
        private readonly ILogger<NFirme> _logger;

        public EfFirmeRepository(EpersContext epersContext,
            IPagination paginationService,
            ILogger<NFirme> logger)
        {
            _epersContext = epersContext;
            _paginationService = paginationService;
            _logger = logger;
        }

        public void Add(NFirme firma)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NFirme.Add(firma);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firme: Add - firma");
                throw;
            }
        }

        public void Update(NFirme firma)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NFirme.Update(firma);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Frime: Update - Firma");
                throw;
            }
        }

        public void SetToInactive(int id)
        {
            try
            {
                var foundFirma = _epersContext.NFirme.Single(frm => frm.Id == id);
                foundFirma.DataSf = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.NFirme.Update(foundFirma);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firme: SetToInactive - Firma");
                throw;
            }
        }

        public NFirme[] GetAll()
        {
            var firme = _epersContext.NFirme.Select(frm => frm).ToArray();

            return firme;
        }

        public NFirme Get(int id)
        {
            try
            {
                var datefirma = from firma in _epersContext.NFirme
                                join locatie in _epersContext.NLocatii on firma.Id
                                equals locatie.IdFirma into locatieGroup
                                from locatie in locatieGroup.DefaultIfEmpty()
                                where firma.Id == id && (locatie == null || locatie.IsSediuPrincipalFirma == true)
                                select new NFirme
                                {
                                    Id = firma.Id,
                                    Denumire = firma.Denumire,
                                    CodFiscal = firma.CodFiscal,
                                    AtributFiscal = firma.AtributFiscal,
                                    TipIntreprindere = firma.TipIntreprindere,
                                    DataIn = firma.DataIn,
                                    DataSf = firma.DataSf,
                                    Activ = firma.DataSf.HasValue && DateTime.Compare(firma.DataSf.Value, DateTime.Now) <= 0 ? false : true,
                                    Adresa =  locatie != null ? locatie.Denumire + ", " + locatie.Adresa + ", " + locatie.Localitate + ", "
                                    + locatie.Judet + ", " + locatie.Tara : ""
                                };

                return datefirma != null ? datefirma.First() : new NFirme();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Frime: Get - Firma");
                throw;
            }
        }

        public FirmeDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            string? filter = null,  int? idFirmaFilter = null)
        {
            var firme = Array.Empty<NFirme>();
            var totalRows = string.IsNullOrWhiteSpace(filter) ? _epersContext.NFirme.Count() :
                _epersContext.NFirme.Count(frm => frm.Denumire.Contains(filter)
                || (frm.CodFiscal != null && frm.CodFiscal.Contains(filter)));

            if (totalRows == 0)
            {
                return new FirmeDisplayModel
                {
                    Firme = firme,
                    CurrentPage = 1,
                    Pages = 1
                };
            }
            var pageSettings = _paginationService.GetPages(currentPage, totalRows, itemsPerPage);

            if (string.IsNullOrWhiteSpace(filter))
                firme = _epersContext.NFirme.Select(frm => new NFirme
                {
                    Id = frm.Id,
                    Denumire = frm.Denumire,
                    CodFiscal = frm.CodFiscal,
                    AtributFiscal = frm.AtributFiscal,
                    TipIntreprindere = frm.TipIntreprindere,
                    DataIn = frm.DataIn,
                    DataSf = frm.DataSf,
                    Activ = frm.DataSf.HasValue && DateTime.Compare(frm.DataSf.Value, DateTime.Now) <= 0 ? false : true
                }).Skip(pageSettings.ItemBeginIndex).Take(pageSettings.DisplayedItems).ToArray();

            else
                firme = _epersContext.NFirme.Where(frm => frm.Denumire.Contains(filter) || frm.CodFiscal.Contains(filter))
                    .Select(frm => new NFirme
                    {
                        Id = frm.Id,
                        Denumire = frm.Denumire,
                        CodFiscal = frm.CodFiscal,
                        AtributFiscal = frm.AtributFiscal,
                        TipIntreprindere = frm.TipIntreprindere,
                        DataIn = frm.DataIn,
                        DataSf = frm.DataSf,
                        Activ = frm.DataSf.HasValue && DateTime.Compare(frm.DataSf.Value, DateTime.Now) <= 0 ? false : true
                    })
                    .Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems).ToArray();

            return new FirmeDisplayModel
            {
                Firme = firme,
                CurrentPage = pageSettings.CurrentPage,
                Pages = pageSettings.Pages
            };
        }
    }
}

