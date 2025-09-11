using Epers.Models.Excel;

namespace EpersBackend.Services.Excel
{
    public interface IExcelRapoarteService
    {
        string SaveExcelRapoarte(ExcelRaportEvaluari[] raportEvaluariList);
    }
}