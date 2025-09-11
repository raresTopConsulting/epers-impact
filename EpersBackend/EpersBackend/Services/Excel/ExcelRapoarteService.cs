using ClosedXML.Excel;
using Epers.Models.Excel;

namespace EpersBackend.Services.Excel
{
    public class ExcelRapoarteService : IExcelRapoarteService
    {
        private readonly IConfiguration _configuration;

        public ExcelRapoarteService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string SaveExcelRapoarte(ExcelRaportEvaluari[] raportEvaluariList)
        {
            var fileSavePath = _configuration["Excel:RaportEvaluariPath"];
            fileSavePath += $"ExcelRaportEvaluari_{raportEvaluariList[0].SesiuneEvaluare}.xlsx";

            // Create a new workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet
                var worksheet = workbook.Worksheets.Add("RaportEvaluari");

                // Define the header row (each property as a column)
                worksheet.Cell(1, 1).Value = "IdAngajat";
                worksheet.Cell(1, 2).Value = "Nume si Prenume";
                worksheet.Cell(1, 3).Value = "Marca";
                worksheet.Cell(1, 4).Value = "Sesiune Evaluare";
                worksheet.Cell(1, 5).Value = "Url";

                int currentRow = 2;
                foreach (var raport in raportEvaluariList)
                {
                    worksheet.Cell(currentRow, 1).Value = raport.IdAngajat;
                    worksheet.Cell(currentRow, 2).Value = raport.NumeSiPrenume;
                    worksheet.Cell(currentRow, 3).Value = raport.Marca;
                    worksheet.Cell(currentRow, 4).Value = raport.SesiuneEvaluare;
                    worksheet.Cell(currentRow, 5).Value = raport.Url;
                    worksheet.Cell(currentRow, 5).SetHyperlink(new XLHyperlink(raport.Url));

                    currentRow++;
                }

                // Save the workbook to a file
                workbook.SaveAs(fileSavePath);

                return $"Excel file saved successfully to {fileSavePath}";
            }
        }
    }
}