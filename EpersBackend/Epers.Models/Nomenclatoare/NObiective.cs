using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
	public class NObiective
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Denumire { get; set; } = string.Empty;

        public int? IdFirma { get; set; }
        public int? IdCompartiment { get; set; }
        public int? IdPost { get; set; }
        public decimal? ValMin { get; set; }
        public decimal? ValTarget { get; set; }
        public decimal? ValMax { get; set; }
        public decimal? BonusMin { get; set; }
        public decimal? BonusTarget { get; set; }
        public decimal? BonusMax { get; set; }
        public string Frecventa { get; set; } = string.Empty;
        public decimal? Pondere { get; set; }
        public bool IsFaraProcent { get; set; }
        public string Tip { get; set; } = string.Empty;
        public string UpdateId { get; set; } = string.Empty;
        public DateTime? UpdateDate { get; set; }
        public bool? IsBonusProcentual { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }
}

