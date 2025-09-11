namespace Epers.Models.Users
{
        public class LoggedInUserData
        {
                public int Id { get; set; }
                public int? IdPost { get; set; }
                public int? IdLocatie { get; set; }
                public int? IdCompartiment { get; set; }
                public int? IdSuperior { get; set; }
                public int? IdFirma { get; set; }
                public int IdRol { get; set; }
                public string Matricola { get; set; } = string.Empty;
                public string Rol { get; set; } = string.Empty;
                public string NumePrenume { get; set; } = string.Empty;
                public string Username { get; set; } = string.Empty;
                public string MatricolaSuperior { get; set; } = string.Empty;
                public string NumeSuperior { get; set; } = string.Empty;
                public int ExpiresIn { get; set; }
                // public string Token { get; set; } = string.Empty;
        }
}
