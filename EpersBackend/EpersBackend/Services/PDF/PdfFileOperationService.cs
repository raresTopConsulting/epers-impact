namespace EpersBackend.Services.PDF
{
    public class PdfFileOperationService : IPdfFileOperationService
    {
        public PdfFileOperationService() {}
        
        public void SavePdfToFolder(byte[] pdfFileBytes, string filePath)
        {
            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write the byte array to a file
                File.WriteAllBytes(filePath, pdfFileBytes);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}