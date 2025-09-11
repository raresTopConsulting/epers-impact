using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Evaluare
{
	public class Notite
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IdSuperior { get; set; } = string.Empty;

        [Required]
        public string MatricolaSuperior { get; set; } = string.Empty;

        [Required]
        public string IdAngajat { get; set; } = string.Empty;

        [Required]
        public string MatricolaAngajat { get; set; } = string.Empty;

        public string Nota { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public string UpdateId { get; set; } = string.Empty;
    }
}

