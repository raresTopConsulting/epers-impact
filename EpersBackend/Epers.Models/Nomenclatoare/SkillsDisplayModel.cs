

namespace Epers.Models.Nomenclatoare
{
	public class SkillsDisplayModel
    {
		public NSkills[] Skills { get; set; } = Array.Empty<NSkills>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

