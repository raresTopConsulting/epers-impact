using Epers.Models.Error;
using Epers.Models.Users;
using EpersBackend.Services.Authenticaion;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.User
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]

    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authService,
            IUserService userService, IConfiguration configuration)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAuthenticationRequest userDto)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Login(userDto);

                if (user == null)
                {
                    return new ObjectResult(new ErrorResponse
                    {
                        Status = 401,
                        Code = "INVALID_CREDENTIALS",
                        Message = "Utilizatorul sau parola introdusă nu sunt corecte!"
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }

                var denumireRol = _userService.GetRol(user.IdRol);
                var accessToken = TokenService.GenerateAccessToken(user.Id, user.IdRol, user.NumePrenume);
                var refreshToken = TokenService.GenerateRefreshToken();

                _userService.SaveRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

                SetAccessTokensInCookie(accessToken);
                SetRefreshTokenInCookie(refreshToken);

                var usrResp = new LoggedInUserData
                {
                    Id = user.Id,
                    Username = user.Username,
                    IdRol = user.IdRol,
                    IdCompartiment = user.IdCompartiment,
                    IdLocatie = user.IdLocatie,
                    IdPost = user.IdPost,
                    IdSuperior = user.IdSuperior,
                    IdFirma = user.IdFirma,
                    Matricola = user.Matricola,
                    NumePrenume = user.NumePrenume,
                    Rol = denumireRol,
                    ExpiresIn = 60 * 60 // time in seconds 60s x 60 = 3.600s
                };

                return Ok(usrResp);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            var oldRefreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(oldRefreshToken))
            {
                return Unauthorized("No refresh token provided.");
            }

            // Validate the refresh token
            var user = _userService.ValidateRefreshToken(oldRefreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            // Check if refresh token is close to expiring
            var tokenExpiry = _userService.GetRefreshTokenExpiry(oldRefreshToken);
            var now = DateTime.UtcNow;
            var timeLeft = tokenExpiry - now;

            if (timeLeft.HasValue && timeLeft.Value.TotalDays < 1)
            {
                var newRefreshToken = TokenService.GenerateRefreshToken();
                _userService.ReplaceRefreshToken(user.Id, oldRefreshToken, newRefreshToken, now.AddDays(7));

                SetRefreshTokenInCookie(newRefreshToken);
            }

            // Generate new tokens
            var newAccessToken = TokenService.GenerateAccessToken(user.Id, user.IdRol, user.NumePrenume);

            SetAccessTokensInCookie(newAccessToken);

            var expiresIn = 60 * 60;

            return Ok(expiresIn);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // RemoveToken();
            var refreshToken = Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("No refresh token found.");
            }

            // Clear cookies
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");

            return Ok();
        }

        private void SetAccessTokensInCookie(string accessToken)
        {
            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };


            Response.Cookies.Append("AccessToken", accessToken, accessCookieOptions);

        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("RefreshToken", refreshToken, refreshCookieOptions);
        }
    }
}

