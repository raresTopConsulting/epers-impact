namespace Epers.Models.PIP
{
    public class ListaSubalterniCalificatiPipModel
    {
        public int IdAngajat { get; set; }
        public int? IdSuperior { get; set; }
        public string Matricola { get; set; } = string.Empty;
        public string? MatricolaSuperior { get; set; } = string.Empty;
        public string NumePrenume { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PostAngajat { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public bool HasPipAprobat { get; set; }
        public string StarePip { get; set;} = string.Empty;
    }

    public class ListaSubalterniCalificatiPipDisplayModel
    {
        public ListaSubalterniCalificatiPipModel[] AngajatiPip { get; set; } = Array.Empty<ListaSubalterniCalificatiPipModel>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}