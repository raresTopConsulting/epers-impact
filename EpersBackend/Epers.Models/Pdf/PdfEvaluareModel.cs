using System;
using Epers.Models.Afisare;
namespace Epers.Models.Pdf
{
	public class PdfEvaluareModel
	{
        public int Anul { get; set; }
        public AfisareSkillsEvalModel DateEvaluare { get; set; } = new AfisareSkillsEvalModel();
        public AfisareHeaderModel Header { get; set; } = new AfisareHeaderModel();
    }
}