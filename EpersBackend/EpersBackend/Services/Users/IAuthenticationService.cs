using Epers.Models.Users;

namespace EpersBackend.Services.Users
{
	public interface IAuthenticationService
	{
        User? Login(UserAuthenticationRequest user);
    }
}

