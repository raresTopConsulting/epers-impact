using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Afisare
{
	public class AfisareHeaderModel
	{
        public int? IdSubaltern { get; set; }

        public int? IdSuperior { get; set; }

        [Display(Name = "Nume și Prenume:")]
        public string NumePrenume { get; set; } = string.Empty;

        [Display(Name = "Matricolă:")]
        public string Matricola { get; set; } = string.Empty;

        [Display(Name = "Nume și Prenume Superior:")]
        public string? NumePrenumeSef { get; set; } = string.Empty;

        [Display(Name = "Matricolă Superior:")]
        public string? MatricolaSef { get; set; } = string.Empty;

        [Display(Name = "Denumire Post:")]
        public string DenumirePost { get; set; } = string.Empty;

        [Display(Name = "COR Post:")]
        public string? COR { get; set; } = string.Empty;

        [Display(Name = "Compartiment:")]
        public string Compartiment { get; set; } = string.Empty;

        [Display(Name = "Locație:")]
        public string Locatie { get; set; } = string.Empty;

        [Display(Name = "Denumire Post:")]
        public string DenumirePostSupervizor { get; set; } = string.Empty;

        [Display(Name = "COR Supervizor:")]
        public string? CORSupervizor { get; set; } = string.Empty;

        [Display(Name = "Compartiment:")]
        public string CompartimentSupervizor { get; set; } = string.Empty;

        [Display(Name = "Locație:")]
        public string LocatieSupervizor { get; set; } = string.Empty;

        public int? IdCompartiment { get; set; }

        public int? IdCompartimentSuperior { get; set; }

        public int? IdPost { get; set; }

        public int? IdPostSuperior { get; set; }

        public int? IdLocatie { get; set; }

        public int? IdLocatieSupervizor { get; set; }
    }
}

