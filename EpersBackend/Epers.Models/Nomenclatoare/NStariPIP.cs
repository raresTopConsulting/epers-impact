using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Nomenclatoare
{
	public class NStariPIP
	{
        [Key]
        public int Id { get; set; }
        public string Denumire { get; set; } = string.Empty;
    }
}

