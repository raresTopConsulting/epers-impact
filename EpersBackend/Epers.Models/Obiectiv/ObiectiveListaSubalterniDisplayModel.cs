
using Epers.Models.Users;

namespace Epers.Models.Obiectiv
{
    public class ObiectiveListaSubalterniDisplayModel
    {
        public SubalterniDropdown[] ListaSubalterni { get; set; } = Array.Empty<SubalterniDropdown>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
