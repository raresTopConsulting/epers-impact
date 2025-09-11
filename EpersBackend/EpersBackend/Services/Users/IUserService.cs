using Epers.Models.Users;

namespace EpersBackend.Services.Users
{
    public interface IUserService
    {
        string GetUsername();

        string GetRol(int idRol);

        bool UserHasRight(string username, string jud);

        User Get(string username);

        User Get(int id);

        User GetUserByMatricola(string matricola);

        User GetSuperior(int idAngajat);

        User GetLoggedInSuperiorUserData(string username);

        User GetLoggedInSuperiorUserData(int id);

        List<User> GetListaSubordonati(int id);

        void Register(UserCreateModel userCreate);

        void Update(UserEditModel user);

        ListaUtilisatoriDisplayModel GetListaUtilizatoriAllFirmePaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? idFirmaFilter = null);

        ListaUtilisatoriDisplayModel GetListaUtilizatoriPaginated(int currentPage, int itemsPerPage, int loggedInUserFirma,
            string? filter = null);

        void Delete(int id);

        void Delete(string matricola);

        List<UserEditModel> GetAll();

        List<UserEditModel> GetAllUsersWithIdFirma();

        List<UserEditModel> GetAllBetween(int idAStart, int idStop);

        List<UserEditModel> GetAllIncompleteUsers();

        SubalterniDropdown[] GetListaSubalterniDropdown(string matricolaSuperior);

        SubalterniDropdown[] GetListaAngajatiForAdminDropdown();

        List<User> GetUseriHR(int? idFirma = null);

        void SaveRefreshToken(int userId, string refreshToken, DateTime expiryDate);

        void ReplaceRefreshToken(int userId, string oldRefreshToken, string newRefreshToken, DateTime expiryDate);

        User? ValidateRefreshToken(string refreshToken);

        DateTime? GetRefreshTokenExpiry(string refreshToken);

        void UpsertResetPasswordToken(int userId, string resetPasswordToken, DateTime expiryDate);
    }
}

