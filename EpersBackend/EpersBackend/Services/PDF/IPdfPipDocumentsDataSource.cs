using Epers.Models.Pdf;

namespace EpersBackend.Services.PDF
{
    public interface IPdfPipDocumentsDataSource
    {
        PdfPipModel GetPdfPipData(int idAngajat, int? anul = null);
    }
}
