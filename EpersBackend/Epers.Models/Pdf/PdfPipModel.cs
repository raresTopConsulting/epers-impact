using Epers.Models.Afisare;
using Epers.Models.PIP;

namespace Epers.Models.Pdf
{
	public class PdfPipModel
	{
        public int Anul { get; set; }
        public AfisareHeaderModel Header { get; set; } = new AfisareHeaderModel();
        public PipDisplayAddEditModel PlanInbunatatirePerformante { get; set; } = new PipDisplayAddEditModel();
    }
}