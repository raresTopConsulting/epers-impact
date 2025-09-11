namespace Epers.Models.PIP
{
    public class ListaSubalterniPipModel
    {
        public int? IdPip { get; set; }
        public int IdAngajat { get; set; }
        public int? IdSuperior { get; set; }
        public string Matricola { get; set; } = string.Empty;
        public string? MatricolaSuperior { get; set; } = string.Empty;
        public string NumePrenume { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public int IdStarePIP { get; set; }
        public string DenumireStarePIP { get; set; } = string.Empty;
    }

    public class ListaSubalterniPipDisplayModel
    {
        public ListaSubalterniPipModel[] AngajatiPip { get; set; } = Array.Empty<ListaSubalterniPipModel>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}