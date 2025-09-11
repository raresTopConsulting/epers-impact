namespace Epers.Models
{
	public class SelectionBox
	{
       public int Id { get; set; }
       public string Sectiune { get; set; } = string.Empty;
       public string Valoare { get; set; } = string.Empty;
    }

    public class SelectionBoxes
    {
        public List<SelectionBox> JudeteSelection = new List<SelectionBox>();
        public List<SelectionBox> TipCompetenteSelection = new List<SelectionBox>();
        public List<SelectionBox> FrecventaObiectiveSelection = new List<SelectionBox>();
        public List<SelectionBox> TipObiectiveSelection = new List<SelectionBox>();
    }

}
