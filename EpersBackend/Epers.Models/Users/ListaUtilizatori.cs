
namespace Epers.Models.Users
{
	public class ListaUtilizatori
	{
		public int Id { get; set; }
		public string Matricola { get; set; } = string.Empty;
        public string Nume_Prenume { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string DenumirePost { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string OrganizatieIntermediara { get; set; } = string.Empty;
        public string OrganizatieBaza { get; set; } = string.Empty;
        public string Locatie { get; set; } = string.Empty;
        public int? IdFirma  { get; set; }
        public string Firma { get; set; } = string.Empty;
    }

    public class ListaUtilisatoriDisplayModel
    {
        public ListaUtilizatori[] Utilizatori { get; set; } = Array.Empty<ListaUtilizatori>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}

