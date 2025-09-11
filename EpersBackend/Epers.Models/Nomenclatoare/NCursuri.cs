using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
    public class NCursuri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Denumire { get; set; } = string.Empty;
       
        public int? IdFirma { get; set; }
        public string? Organizator { get; set; } = string.Empty;
        public decimal? Pret { get; set; }
        public DateTime? DataInceput {  get; set; }
        public DateTime? DataSfarsit { get; set; }
        public string? Locatie { get; set; } = string.Empty;
        public bool? IsOnline { get; set; }
        public string? Link { get; set; } = string.Empty;

        [NotMapped]
        public bool Activ { get; set; }

        [NotMapped]
        public string Firma { get; set; } = string.Empty;

    }
}
