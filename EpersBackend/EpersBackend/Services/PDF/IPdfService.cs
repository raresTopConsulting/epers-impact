
namespace EpersBackend.Services.PDF
{
	public interface IPdfService
	{
        byte[] GeneratePDFEvaluare(int idAngajat, int? anul = null);
        byte[] GeneratePDFObiectiveActuale(int idAngajat, int? anul = null);
        byte[] GeneratePDFObiectiveIstoric(int idAngajat, int? anul = null);
        byte[] GeneratePDFEvaluareSiConcluzii(int idAngajat, int? anul = null);
        byte[] GeneratePDFPip(int idAngajat, int? anul = null);
    }
}

