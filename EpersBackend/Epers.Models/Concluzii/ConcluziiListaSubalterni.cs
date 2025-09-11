using System;

namespace Epers.Models.Concluzii
{
	public class ConcluziiListaSubalterni
	{
        public string NumePrenume { get; set; } = string.Empty;
        public int IdAngajat { get; set; }
        public string MatricolaAngajat { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string COR { get; set; } = string.Empty;
        public string DataUltimaEval { get; set; } = string.Empty;
        public string DataEvalFin { get; set; } = string.Empty;
        public bool FlagFinalizat { get; set; }
        public string Concluzii { get; set; } = string.Empty;
    }

    public class ConcluziiListaSubalterniDisplayModel
    {
        public ConcluziiListaSubalterni[] ListaSubalterni { get; set; } = Array.Empty<ConcluziiListaSubalterni>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

