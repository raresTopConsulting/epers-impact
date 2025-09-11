using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NCompartimente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Denumire { get; set; } = string.Empty;

        public int? IdFirma { get; set; }
        public int? Id_Locatie { get; set; }
        public DateTime? Data_in { get; set; }
        public DateTime? Data_sf { get; set; }
        public int? Sus { get; set; }
        public int? Jos { get; set; }
        public string? SubCompartiment { get; set; }

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;

    }
}

