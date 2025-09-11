using System;

namespace Epers.Models.Obiectiv
{
	public class ObiectiveDisplayModel
	{
        public Obiective[] ListaObActuale { get; set; } = Array.Empty<Obiective>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

