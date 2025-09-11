using Epers.Models.Pdf;

namespace EpersBackend.Services.PDF
{
    public interface IPdfObiectiveDocumentsDataSource
    {
        PdfObiectiveModel GetPdfObActualeData(int idAngajat, int? anul = null);
        PdfObiectiveModel GetPdfObIstoricData(int idAngajat, int? anul = null);
    }
}
