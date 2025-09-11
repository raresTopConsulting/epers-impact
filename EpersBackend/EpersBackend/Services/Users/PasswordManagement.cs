using Epers.DataAccess;
using Epers.Models.Users;
using EpersBackend.Services.Authenticaion;

namespace EpersBackend.Services.Users
{
    public class PasswordManagement : IPasswordManagement
    {
        private readonly EpersContext _epersContext;
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly ILogger<PasswordManagement> _logger;

        public PasswordManagement(EpersContext epersContext,
            IPasswordService passwordService,
            IUserService userService,
            ILogger<PasswordManagement> logger)
        {
            _epersContext = epersContext;
            _passwordService = passwordService;
            _userService = userService;
            _logger = logger;
        }

        public bool ChangePassword(string newPassword, int userId)
        {
            try
            {
                var user = _userService.Get(userId);
                if (user != null)
                {
                    _passwordService.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.VerifiedAt = DateTime.Now;

                    if (user.ResetPasswordToken != null) 
                        user.ResetPasswordToken = null;
                    
                    if (user.ResetPasswordTokenExpires != null) 
                        user.ResetPasswordTokenExpires = null;   

                    using (var dbTransaction = _epersContext.Database.BeginTransaction())
                    {
                        _epersContext.User.Update(user);
                        _epersContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                    return true;
                }
                else
                    return false;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Password Management: Change password");
                throw;
            }
        }

        public string GenerateAlternativePassword(User user)
        {
            var resetPasswordToken = TokenService.GenerateTemporaryPasswordToken();
            var resetPasswordTokenExpieres = DateTime.UtcNow.AddHours(2);

            _userService.UpsertResetPasswordToken(user.Id, resetPasswordToken, resetPasswordTokenExpieres);

            return resetPasswordToken ?? string.Empty;
        }
    }
}

