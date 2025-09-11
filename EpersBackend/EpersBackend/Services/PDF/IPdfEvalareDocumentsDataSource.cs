using Epers.Models.Pdf;

namespace EpersBackend.Services.PDF
{
    public interface IPdfEvaluareDocumentsDataSource
    {
        PdfEvaluareModel GetPdfEvaluareData(int idAngajat, int? anul = null);
    }
}
