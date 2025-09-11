using System;
using Epers.Models.Nomenclatoare;

namespace Epers.Models.Afisare
{
	public class TableSetareProfil
	{
		public SetareProfil SetareProfil { get; set; } = new SetareProfil();
		public string DenumireSkill { get; set; } = string.Empty;
		public string DescriereSkill { get; set; } = string.Empty;
	}
}
