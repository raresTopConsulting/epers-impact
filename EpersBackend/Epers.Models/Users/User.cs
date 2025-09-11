using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Users
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } = new byte[64];

        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[128];

        [Required]
        public string Matricola { get; set; } = string.Empty;

        [Required]
        public string NumePrenume { get; set; } = string.Empty;

        public int IdRol { get; set; }
        public int? IdPost { get; set; }
        public int? IdLocatie { get; set; }
        public int? IdCompartiment { get; set; }
        public int? IdSuperior { get; set; }
        public int? IdFirma { get; set; }

        public string? MatricolaSuperior { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpires { get; set; }
        
        [NotMapped]
        public string Firma { get; set; } = string.Empty;
    }
}
