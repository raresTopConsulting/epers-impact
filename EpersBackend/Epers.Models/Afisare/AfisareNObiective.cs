namespace Epers.Models.Afisare
{
	public class AfisareNObiective
	{
        public int Id { get; set; }
        public string Post { get; set; } = string.Empty;
        public string Compartiment { get; set; } = string.Empty;
        public string Denumire { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public decimal? ValTarget { get; set; }
        public decimal? ValMin { get; set; }
        public decimal? ValMax { get; set; }
        public decimal? BonusTarget { get; set; }
        public decimal? BonusMin { get; set; }
        public decimal? BonusMax { get; set; }
        public string? Frecventa { get; set; }
        public bool IsFaraProcent { get; set; }
        public bool? IsBonusProcentual { get; set; }
        public string Firma { get; set; } = string.Empty;
        public int? IdFirma { get; set; } 
    }

    public class AfisareNObiectiveDisplayModel
    {
        public AfisareNObiective[] AfisNomObiectiveData { get; set; } = Array.Empty<AfisareNObiective>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

