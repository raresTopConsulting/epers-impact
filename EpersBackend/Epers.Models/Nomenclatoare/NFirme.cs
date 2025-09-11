using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NFirme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Denumire { get; set; } = string.Empty;

        public string CodFiscal { get; set; } = string.Empty;
        public string TipIntreprindere { get; set; } = string.Empty;
        public string AtributFiscal { get; set; } = string.Empty;
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }
        public string? UpdateId { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public string? Adresa { get; set; } = string.Empty;

        [NotMapped]
        public bool Activ { get; set; }
    }
}

