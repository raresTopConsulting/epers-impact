using Epers.DataAccess;
using Epers.Models.Users;

namespace EpersBackend.Services.Users
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly EpersContext _epersDbContext;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(EpersContext epersContext,
            IPasswordService passwordService)
		{
            _epersDbContext = epersContext;
            _passwordService = passwordService;
		}

        public User? Login(UserAuthenticationRequest userAuthRequest)
        {
            var user = _epersDbContext.User.SingleOrDefault(u => u.Username == userAuthRequest.Username);

            if (user == null) return null;

            if (user.VerifiedAt == null) return null;

            if (!_passwordService.VerifyPasswordHash(userAuthRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                // login with temporary token
                if (user.ResetPasswordToken == userAuthRequest.Password)
                {
                    if (user.ResetPasswordTokenExpires < DateTime.UtcNow)
                    {
                        throw new Exception("Token-ul de acces a expirat! Va rugam sa generati unul nou!");
                    }
                    return user;
                }
                return null;
            }
            
            return user;
        }
    }
}

