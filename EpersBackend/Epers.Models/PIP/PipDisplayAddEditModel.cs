namespace Epers.Models.PIP
{
    public class PipDisplayAddEditModel : PlanInbunatatirePerformante
    {
        public string NumePrenumeAngajat { get; set; } = string.Empty;
        public string NumePrenumeSuperior { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string? PostSuperior { get; set; }
        public string DenumireStare { get; set; } = string.Empty;
    }
}