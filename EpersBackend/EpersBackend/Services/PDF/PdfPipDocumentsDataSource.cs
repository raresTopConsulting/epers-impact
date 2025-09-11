using Epers.Models.Pdf;
using EpersBackend.Services.Header;
using EpersBackend.Services.PIP;

namespace EpersBackend.Services.PDF
{
    public class PdfPipDocumentsDataSource : IPdfPipDocumentsDataSource
    {
        private readonly IPIPService _pipService;
        private readonly IHeaderService _headerService;

        public PdfPipDocumentsDataSource(IPIPService pipService, IHeaderService headerService)
        {
            _pipService = pipService;
            _headerService = headerService;
        }

        public PdfPipModel GetPdfPipData(int idAngajat, int? anul = null)
        {
            var headerData = _headerService.GetHeader(idAngajat);
            var pip = _pipService.Get(idAngajat, anul);

            return new PdfPipModel
            {
                Header = headerData,
                PlanInbunatatirePerformante = pip,
                Anul = anul.HasValue? anul.Value : DateTime.Now.Year
            };
        }
    }
}