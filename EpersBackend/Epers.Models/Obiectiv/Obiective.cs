using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Obiectiv
{
    public class Obiective
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Denumire { get; set; } = string.Empty;

        public string IdAngajat { get; set; } = string.Empty;

        public string MatricolaAngajat { get; set; } = string.Empty;

        public string IdSuperior { get; set; } = string.Empty;

        public string MatricolaSuperior { get; set; } = string.Empty;

        public int? IdCompartiment { get; set; }

        public int? IdPost { get; set; }

        public DateTime? DataIn { get; set; }

        public DateTime? DataSf { get; set; }

        public decimal? ValMin { get; set; }

        public decimal? ValTarget { get; set; }

        public decimal? ValMax { get; set; }

        public decimal? BonusMin { get; set; }

        public decimal? BonusTarget { get; set; }

        public decimal? BonusMax { get; set; }

        public string Frecventa { get; set; } = string.Empty;

        public string Tip { get; set; } = string.Empty;

        public string ValoareRealizata { get; set; } = string.Empty;

        public bool? IsRealizat { get; set; }

        public decimal? Pondere { get; set; }

        public bool? IsActive { get; set; }

        public bool IsFaraProcent { get; set; }

        public bool? IsBonusProcentual { get; set; }

        public string UpdateId { get; set; } = string.Empty;

        public DateTime? UpdateDate { get; set; }

        public int? IdFirma { get; set; }
        
        public bool? IsInDesfasurare { get; set; }
    }
}

