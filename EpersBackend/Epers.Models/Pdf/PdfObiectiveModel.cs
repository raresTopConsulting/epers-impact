using Epers.Models.Afisare;
using Epers.Models.Obiectiv;

namespace Epers.Models.Pdf
{
	public class PdfObiectiveModel
	{
        public int Anul { get; set; }
        public Obiective[] DateObiective { get; set; } = Array.Empty<Obiective>();
        public AfisareHeaderModel Header { get; set; } = new AfisareHeaderModel();
    }
}