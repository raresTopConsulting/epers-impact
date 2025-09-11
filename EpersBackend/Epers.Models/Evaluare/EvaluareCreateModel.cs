
namespace Epers.Models.Evaluare
{
    public class EvaluareCreateModel
    {
        public int IdSkill { get; set; }
        public int Ideal { get; set; }
        public int? ValIndiv { get; set; }
        public int? Val { get; set; }
        public int? ValFin { get; set; }
        public string? Obs { get; set; }
    }
}

