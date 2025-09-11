using System;
using Epers.Models.Nomenclatoare;

namespace Epers.Models.Nomenclatoare
{
	public class PosturiDisplayModel
    {
		public NPosturi[] Posturi { get; set; } = Array.Empty<NPosturi>();
		public int Pages { get; set; }
		public int CurrentPage { get; set; }
	}
}

