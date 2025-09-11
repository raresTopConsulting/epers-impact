using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NDivizii
    {
        [Key]
        public int Id { get; set; }

        public int? IdFirma { get; set; }
        public string? Denumire { get; set; }
        public string? Descriere { get; set; }
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }
        public string? UpdateId { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }
}

