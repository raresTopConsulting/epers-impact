namespace Epers.Models.Afisare
{
	public class AfisareUserDetails
	{
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string NumePrenume { get; set; } = string.Empty;
        public string Matricola { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string DenumirePost { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string OrganizatieIntermediara { get; set; } = string.Empty;
        public string OrganizatieBaza { get; set; } = string.Empty;
        public string Locatie { get; set; } = string.Empty;
        public string NumeSuperior { get; set; } = string.Empty;
    }
}

