
namespace Epers.Models.Nomenclatoare
{
    public class CursuriDisplayModel
    {
        public NCursuri[] Cursuri { get; set; } = Array.Empty<NCursuri>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
