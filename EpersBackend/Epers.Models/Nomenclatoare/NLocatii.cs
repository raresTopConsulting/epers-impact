using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NLocatii
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Denumire { get; set; } = string.Empty;

        public int? IdFirma { get; set; }
        public bool? IsSediuPrincipalFirma { get; set; }
        public string? Adresa { get; set; }
        public string? Localitate { get; set; }
        public string? Judet { get; set; }
        public string? Tara { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }

}

