using System;
using System.ComponentModel.DataAnnotations;

namespace Epers.Models.Competente
{
	public class RelevantSkillsModel
	{
        public int Id_Skill { get; set; }
        public string Denumire { get; set; } = string.Empty;
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }

        [StringLength(2000)]
        public string? Detalii { get; set; } = string.Empty;

        public int IdPost { get; set; }
        public int Ideal { get; set; }
    }
}

