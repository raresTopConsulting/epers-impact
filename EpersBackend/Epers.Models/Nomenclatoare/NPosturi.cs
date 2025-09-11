using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NPosturi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nume { get; set; } = string.Empty;

        public int? IdFirma { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }

        public string? ProfilCompetente { get; set; }
        public string? COR { get; set; }
        public string? DenFunctie { get; set; }
        public string? NivelPost { get; set; }
        public int? Punctaj { get; set; }

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }
}

