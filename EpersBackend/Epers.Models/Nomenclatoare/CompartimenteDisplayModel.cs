
namespace Epers.Models.Nomenclatoare
{
	public class CompartimenteDisplayModel
    {
		public NCompartimentDisplay[] Compartimente { get; set; } = Array.Empty<NCompartimentDisplay>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

