using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NSkills
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string Denumire { get; set; } = string.Empty;

        [StringLength(800)]
        [Required]
        public string Descriere { get; set; } = string.Empty;

        public int? IdFirma { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }
        public char? Tip { get; set; }
        public string? Detalii { get; set; }

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }
}

