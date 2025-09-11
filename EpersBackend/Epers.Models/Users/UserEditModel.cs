using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Users
{
	public class UserEditModel
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string NumePrenume { get; set; } = string.Empty;

        [Required]
        public string Matricola { get; set; } = string.Empty;

        public int IdRol { get; set; }
        public int? IdPost { get; set; }
        public int? IdLocatie { get; set; }
        public int? IdCompartiment { get; set; }
        public int? IdSuperior { get; set; }
        public int? IdFirma { get; set; }
        public string MatricolaSuperior { get; set; } = string.Empty;
    }
}

