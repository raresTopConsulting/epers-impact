using Epers.Models.Pdf;

namespace EpersBackend.Services.PDF
{
    public interface IPdfEvalareDocumentsConclzuieDataSource
    {
        PdfEvaluareConcluziiModel GetPdfEvaluareSiConcluziiData(int idAngajat, int? anul = null);
    }
}
