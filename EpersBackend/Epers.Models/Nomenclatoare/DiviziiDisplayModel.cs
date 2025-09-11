
namespace Epers.Models.Nomenclatoare
{
	public class DiviziiDisplayModel
    {
		public NDivizii[] Divizii { get; set; } = Array.Empty<NDivizii>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

