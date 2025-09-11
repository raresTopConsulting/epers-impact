using Epers.Models.Afisare;
using Epers.Models.Concluzii;
using Epers.Models.Nomenclatoare;
namespace Epers.Models.Pdf
{
	public class PdfEvaluareConcluziiModel
	{
        public int Anul { get; set; }
        public AfisareSkillsEvalModel DateEvaluare { get; set; } = new AfisareSkillsEvalModel();
        public Concluzie? ConclzuieEvaluare { get; set; } = new Concluzie();
        public NCursuri[] ListaTrainiguri { get; set; } = Array.Empty<NCursuri>();
        public AfisareHeaderModel Header { get; set; } = new AfisareHeaderModel();
    }
}