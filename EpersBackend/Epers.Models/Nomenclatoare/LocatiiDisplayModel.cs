
namespace Epers.Models.Nomenclatoare
{
	public class LocatiiDisplayModel
	{
		public NLocatii[] Locatii { get; set; } = Array.Empty<NLocatii>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

