
namespace Epers.Models
{
	public class DropdownSelection
	{
		public int Id { get; set; }
		public string Text { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
		public int? IdFirma { get; set; }
	}

	public class AppDropdownSelections
	{
		public List<DropdownSelection> DdUseri  { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdPosturi { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdCompartimente { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdCompetente { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdDivizii { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdLocatii { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdRoluri { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdObiective { get; set; } = new List<DropdownSelection>();
		public List<DropdownSelection> DdCursuri { get; set; } = new List<DropdownSelection>();
	}
}

