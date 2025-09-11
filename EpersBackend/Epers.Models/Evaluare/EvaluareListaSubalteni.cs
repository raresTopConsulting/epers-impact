
using System.Text;

namespace Epers.Models.Evaluare
{
	public class EvaluareListaSubalteni
	{
        public string NumePrenume { get; set; } = string.Empty;
        public int IdAngajat { get; set; }
        public string MatricolaAngajat { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string COR { get; set; } = string.Empty;
        public string DataUltimaEval { get; set; } = string.Empty;
        public string DataEvalSef { get; set; } = string.Empty;
        public string DataAutoEval { get; set; } = string.Empty;
        public string DataEvalFin { get; set; } = string.Empty;
        public bool FlagFinalizat { get; set; }
        public string Concluzii { get; set; } = string.Empty;
        public bool FinaliztAnulCurent { get; set; }
    }

    public class EvaluareListaSubalterniDisplayModel
    {
        public EvaluareListaSubalteni[] ListaSubalterni { get; set; } = Array.Empty<EvaluareListaSubalteni>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class SpEvaluareListaSubalterni
    {
        public string MatricolaAngajat { get; set; } = string.Empty;
        public string NumePrenume { get; set; } = string.Empty;
        public int IdAngajat { get; set; }
        public string? PostAngajat { get; set; }
        public string? COR { get; set; }
        public string? DataAutoEval { get; set; }
        public string? DataEvalSef { get; set; }
        public string? DataEvalFin { get; set; }
        public string? DataUltimaEval { get; set; }
        public int FlagFinalizat { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

