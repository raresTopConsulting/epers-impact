using EpersBackend.Services.Email;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Email
{
    [Route("api/EmailObiective")]
    [Authorize]
    [ApiController]

    public class EmailObiectiveController : ControllerBase
    {
        private readonly IEmailSendService _emailSendService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public EmailObiectiveController(IConfiguration configuration,
            IEmailSendService emailSendService,
            IUserService userService)
        {
            _configuration = configuration;
            _emailSendService = emailSendService;
            _userService = userService;
        }

        [HttpGet]
        [Route("obiectiveSetate")]
        public IActionResult SendEmailObiectiveSetate([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);

            if (sendEmail)
            {
                _emailSendService.SendEmailObiectiveSetate(angajat.NumePrenume, angajat.Username, angajat.Id);

                return Ok(new
                {
                    message = "Email-ul a fost trimis la adresa: " + angajat.Username + "!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet]
        [Route("obiectiveEvaluate")]
        public IActionResult SendEmailObiectiveEvaluate([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);

            if (sendEmail)
            {
                _emailSendService.SendEmailObiectiveEvaluate(angajat.NumePrenume, angajat.Username, angajat.Id);

                return Ok(new
                {
                    message = "Email-ul a fost trimis la adresa: " + angajat.Username + "!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }
    }
}