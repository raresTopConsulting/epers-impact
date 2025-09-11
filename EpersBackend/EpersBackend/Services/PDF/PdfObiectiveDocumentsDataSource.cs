using Epers.Models.Pdf;
using EpersBackend.Services.Header;
using EpersBackend.Services.ObiectivService;

namespace EpersBackend.Services.PDF
{
    public class PdfObiectiveDocumentsDataSource : IPdfObiectiveDocumentsDataSource
    {
        private readonly IObiectiveService _obiectiveService;
        private readonly IHeaderService _headerService;

        public PdfObiectiveDocumentsDataSource(IObiectiveService obiectiveService, IHeaderService headerService)
        {
            _obiectiveService = obiectiveService;
            _headerService = headerService;
        }

        public PdfObiectiveModel GetPdfObActualeData(int idAngajat, int? anul = null)
        {
            var headerData = _headerService.GetHeader(idAngajat);
            var obActuale = _obiectiveService.GetObiectiveActuale(idAngajat, null);

            return new PdfObiectiveModel
            {
                Anul = anul.HasValue ? anul.Value : DateTime.Now.Year,
                DateObiective = obActuale,
                Header = headerData
            };
        }

        public PdfObiectiveModel GetPdfObIstoricData(int idAngajat, int? anul = null)
        {
            var headerData = _headerService.GetHeader(idAngajat);
            var istoricOb = _obiectiveService.GetIstoricObiective(idAngajat, null);

            return new PdfObiectiveModel
            {
                Anul = anul.HasValue ? anul.Value : DateTime.Now.Year,
                DateObiective = istoricOb,
                Header = headerData
            };
        }
    }
}