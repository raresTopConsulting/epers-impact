using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epers.Models.Nomenclatoare
{
	public class SetareProfil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Id_Skill { get; set; }
        public int? Id_Post { get; set; }
        public int? Ideal { get; set; }
    }
}

