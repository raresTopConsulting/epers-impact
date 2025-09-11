namespace Epers.Models.Nomenclatoare
{
	public class FirmeDisplayModel
    {
		public NFirme[] Firme { get; set; } = Array.Empty<NFirme>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

