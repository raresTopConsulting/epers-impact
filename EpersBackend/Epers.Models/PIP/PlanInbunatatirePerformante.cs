using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.PIP
{
    public class PlanInbunatatirePerformante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? IdAngajat { get; set; }
        public string? Matricola { get; set; } = string.Empty;
        public int? IdSuperior { get; set; }
        public string? MatricolaSuperior { get; set; } = string.Empty;
        public int? IdPost { get; set; }
        public int? IdPostSuperior { get; set; }
        public DateTime? DataInceputEvaluare { get; set; }
        public DateTime? DataSfarsitEvaluare { get; set; }
        public decimal CalificativEvaluare { get; set; }
        public DateTime DataInceputPip { get; set; }
        public DateTime DataSfarsitPip { get; set; }
        public decimal CalificativMinimPip { get; set; }
        public string? ObiectiveDezvoltare { get; set; }
        public string? ActiuniSalariat { get; set; }
        public string? SuportManager { get; set; }
        public string? AltSuport { get; set; }
        public decimal? CalificativFinalPip { get; set; }
        public string? ObservatiFinalPip { get; set; }
        public string? DeczieFinalaManager { get; set; }
        public int IdStare { get; set; }
        public string ObservatiiHr { get; set; } = string.Empty;
        public int Anul { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateId { get; set; } = string.Empty;
        public int? IdFirma { get; set; }
    }
}