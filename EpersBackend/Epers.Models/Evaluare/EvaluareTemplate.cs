
namespace Epers.Models.Evaluare
{
	public class EvaluareTemplate
	{
        public int IdAngajat { get; set; }
        public int IdPost { get; set; }
        public int TipEvaluare { get; set; }
        public List<EvaluareCreateModel> DateEval { get; set; } = new List<EvaluareCreateModel>();
        public int Anul {get; set; }
    }
}

