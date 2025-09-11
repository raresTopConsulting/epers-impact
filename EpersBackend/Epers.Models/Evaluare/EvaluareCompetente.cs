using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Evaluare
{
    public class Evaluare_competente
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        public int Anul { get; set; }

        public DateTime? Data_EvalSef { get; set; }
        public DateTime? Data_AutoEval { get; set; }
        public DateTime? Data_EvalFinala { get; set; }
        public string Id_angajat { get; set; } = string.Empty;
        public string Id_sef { get; set; } = string.Empty;
        public string Matricola { get; set; } = string.Empty;
        public string Matricola_Sef { get; set; } = string.Empty;

        [Required]
        public int Id_skill { get; set; }

        public int? Id_post { get; set; }
        public string? Valoare_indiv { get; set; }
        public string? Valoare { get; set; }
        public string? Valoare_fin { get; set; }

        [Required]
        public int Flag_finalizat { get; set; }

        public string? Observatie { get; set; }
        public string? ConcluziiEvalCantOb { get; set; }
        public string? ConcluziiEvalCompActDezProf { get; set; }
        public string? ConcluziiAspecteGen { get; set; }
        public string? Id_training { get; set; }
        public int? IdFirma { get; set; }
        public string? UpdateId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? CalificativFinalEvaluare { get; set; }
    }
}

