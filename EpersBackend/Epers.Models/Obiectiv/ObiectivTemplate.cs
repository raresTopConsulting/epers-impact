using System;
namespace Epers.Models.Obiectiv
{
    public class ObiectivTemplate
    {
        public int Id { get; set; }
        public string Denumire { get; set; } = string.Empty;
        public decimal? ValMin { get; set; }
        public decimal? ValTarget { get; set; }
        public decimal? ValMax { get; set; }
        public decimal? BonusMin { get; set; }
        public decimal? BonusTarget { get; set; }
        public decimal? BonusMax { get; set; }
        public bool IsFaraProcent { get; set; }
        public string Tip { get; set; } = string.Empty;
        public DateTime? DataIn { get; set; }
        public DateTime? DataSf { get; set; }
        public string Frecventa { get; set; } = string.Empty;
        public bool? IsBonusProcentual { get; set; }
    }
}
