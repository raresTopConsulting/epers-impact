using System;
using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Users
{
	public class ResetPasswordRequest
	{
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(3)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

