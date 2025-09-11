namespace EpersBackend.Services.PDF
{
	public interface IPdfFileOperationService
	{
        void SavePdfToFolder(byte[] pdfFileBytes, string filePath);
    }
}

