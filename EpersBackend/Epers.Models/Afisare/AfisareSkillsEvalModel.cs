namespace Epers.Models.Afisare
{
	public class AfisareSkillsEvalModel
	{
        public int IdPost { get; set; }
        public bool FlagFinalizat { get; set; }
        public List<EvalArray> DateEval { get; set; } = new List<EvalArray>();

        public class EvalArray
        {
            public int IdSkill { get; set; }
            public string DenumireSkill { get; set; } = string.Empty;
            public string DetaliiSkill { get; set; } = string.Empty;
            public int Ideal { get; set; }
            public int? ValIndiv { get; set; }
            public int? Val { get; set; }
            public int? ValFin { get; set; }
            public string? Obs { get; set; }
            public DateTime? DataEvalFinala { get; set;}
        }
        public int[]? DisplayIdsTraining { get; set; }
        public decimal? CalificativFinal {get; set; }
        public string IncadrareCalificativFinal {get; set; } = string.Empty;
    }
}

