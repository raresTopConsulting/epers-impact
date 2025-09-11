using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Users
{
	public class PasswordChange
	{
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        //[DataType(DataType.Password)]
        //[Required]
        //public string OldPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required]
        public string NewPass { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required]
        public string ConfNewPass { get; set; } = string.Empty;
    }
}

