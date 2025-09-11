namespace Epers.Models.Evaluare
{
    public class EvaluarePipData
    {
        public long[] IdEvaluari { get; set; } = Array.Empty<long>();
        public DateTime DataInceputEvaluare { get; set; }
        public DateTime DataSfarsitEvaluare { get; set; }
        public decimal CalificativEvaluare { get; set; }
    }
}

