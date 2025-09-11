using Epers.Models.Pdf;
using EpersBackend.Services.Evaluare;
using EpersBackend.Services.Header;

namespace EpersBackend.Services.PDF
{
    public class PdfEvaluareDocumentsDataSource : IPdfEvaluareDocumentsDataSource
    {
        private readonly IEvaluareService _evaluareService;
        private readonly IHeaderService _headerService;

        public PdfEvaluareDocumentsDataSource(IEvaluareService evaluareService, IHeaderService headerService)
        {
            _evaluareService = evaluareService;
            _headerService = headerService;
        }

        public PdfEvaluareModel GetPdfEvaluareData(int idAngajat, int? anul = null)
        {
            var headerData = _headerService.GetHeader(idAngajat);
            var dateEvaluare = _evaluareService.GetIstoricEvalTemplate(idAngajat, anul);

            return new PdfEvaluareModel
            {
                Anul = anul.HasValue ? anul.Value : DateTime.Now.Year,
                DateEvaluare = dateEvaluare,
                Header = headerData
            };
        }
    }
}