using Epers.Models.Users;

namespace EpersBackend.Services.Users
{
	public interface IPasswordManagement
	{
        string GenerateAlternativePassword(User user);

        bool ChangePassword(string newPassword, int userId);
    }
}

