using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Users
{
	public class UserAuthenticationRequest
	{
        [Required, EmailAddress]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

