namespace Epers.Models.Excel
{
    public class ExcelRaportEvaluari
    {
        public int IdAngajat { get; set; }
        public string NumeSiPrenume {get; set;} = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int SesiuneEvaluare { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}