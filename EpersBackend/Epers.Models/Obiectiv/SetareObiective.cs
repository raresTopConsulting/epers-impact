using System;

namespace Epers.Models.Obiectiv
{
	public class SetareObiective
	{
        public string IdAngajat { get; set; } = string.Empty;
        public string MatricolaAngajat { get; set; } = string.Empty;
        public string IdSuperior { get; set; } = string.Empty;
        public string MatricolaSuperior { get; set; } = string.Empty;
        public int? IdCompartiment { get; set; }
        public int? IdPost { get; set; }
        public string UpdateId { get; set; } = string.Empty;
        public DateTime? UpdateDate { get; set; }

        public List<ObiectivTemplate> ObiectivTemplate { get; set; } = new List<ObiectivTemplate>();
    }
}

