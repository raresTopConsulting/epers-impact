namespace Epers.Models.Users
{
    public class SubalterniDropdown
    {
        public int IdAngajat { get; set; }
        public string MatricolaAngajat { get; set; } = string.Empty;
        public string NumePrenume { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string COR { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public bool? HideStartPip { get; set; }
    }
}