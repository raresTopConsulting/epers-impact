using System;
namespace Epers.Models.Users
{
	public class NomenclatoareSelection
	{
		public List<DropdownSelection> PosturiSelection { get; set; } = new List<DropdownSelection>();
        public List<DropdownSelection> LocatiiSelection { get; set; } = new List<DropdownSelection>();
        public List<DropdownSelection> CompartimenteSelection { get; set; } = new List<DropdownSelection>();
        public List<DropdownSelection> CompetenteSelection { get; set; } = new List<DropdownSelection>();

    }
}

