using Epers.Models.Users;
using EpersBackend.Services.Email;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.User
{
    [ApiController]
    [Route("api/UserPasswordManagement")]

    public class UserPasswordManagementController : ControllerBase
    {
        private readonly IPasswordManagement _passwordManagement;
        private readonly IUserService _userService;
        private readonly IEmailSendService _emailSendService;

        public UserPasswordManagementController(IPasswordManagement passwordManagement,
            IEmailSendService emailSendService,
            IUserService userService)
        {
            _passwordManagement = passwordManagement;
            _emailSendService = emailSendService;
            _userService = userService;
        }

        [Authorize]
        [HttpPut("changePassword")]
        public IActionResult UpdatePassword([FromBody] PasswordChange passwordChange)
        {
            if (ModelState.IsValid)
            {
                if (passwordChange.NewPass != passwordChange.ConfNewPass)
                    return BadRequest("Parolele introduse nu sunt identice!");

                var success = _passwordManagement.ChangePassword(passwordChange.NewPass, passwordChange.UserId);

                if (success == true)
                    return Ok(new { message = "Parola a fost schimbată cu succes! Vă rugăm să închideți fereastra de dialog!", statusCode = 200 });
                else
                    return StatusCode(500, new { message = "Eroare la schimbarea parolei!" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("forgotPassword")]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            var user = _userService.Get(email);
            if (user == null)
            {
                return Unauthorized( new { message = "Adresa de email este invalida!" });
            }
            var tempPassCode = _passwordManagement.GenerateAlternativePassword(user);
            _emailSendService.SendEmailWithTemporaryPassword(user, tempPassCode);

            return Ok( new { message = "A fost trimis un email pentru resetarea parolei!" });
        }

        [HttpPost("restAllPasswords")]
        public IActionResult RestAllPasswords()
        {
            throw new NotImplementedException();
        }
    }
}

