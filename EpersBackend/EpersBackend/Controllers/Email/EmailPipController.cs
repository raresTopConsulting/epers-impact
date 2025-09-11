using EpersBackend.Services.Email;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Email
{
    [Route("api/EmailPip")]
    [Authorize]
    [ApiController]

    public class EmailPipController : ControllerBase
    {
        private readonly IEmailSendService _emailSendService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public EmailPipController(IEmailSendService emailSendService,
            IUserService userService,
            IConfiguration configuration)
        {
            _emailSendService = emailSendService;
            _configuration = configuration;
            _userService = userService;
        }

        // void SendEmailPIPUpdate(string numeAngajatPip, string mailAngajat, string numeManager, string mailHr);
        // void SendEmailPIPIncheiat(string numeAngajatPip, string mailAngajat, string numeManager, string mailHr);

        [HttpGet("calificat")]
        public IActionResult SendEmaiCalificatPipToHRandAngajat([FromQuery] int idAngajat, [FromQuery] int? idFrima)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);
            var useriHr = _userService.GetUseriHR(idFrima);

            if (sendEmail)
            {
                foreach (var userHr in useriHr)
                {
                    _emailSendService.SendEmailCalificatPIPToHR(angajat.NumePrenume, userHr.Username, userHr.NumePrenume);
                }
                _emailSendService.SendEmailCalificatPIPToAngajat(angajat.NumePrenume, angajat.Username, angajat.Id);

                return Ok(new
                {
                    message = "Email-ul a fost trimis la departamentul de Resurse Umane!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("aprobat")]
        public IActionResult SendEmaiPipAprobatToAngajatAndManager([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);
            var manager = _userService.GetSuperior(idAngajat);

            if (sendEmail)
            {
                _emailSendService.SendEmailAprobarePIPTOAngajat(angajat.NumePrenume, angajat.Username, angajat.Id);
                _emailSendService.SendEmailAprobarePIPToManager(angajat.NumePrenume, manager.Username, manager.NumePrenume);

                return Ok(new
                {
                    message = "E-mail-ul a fost trimis la departamentul de Resurse Umane si la managerul direct!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("respins")]
        public IActionResult SendEmaiPipRespinsToManagerAndAngajat([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);
            var manager = _userService.GetSuperior(idAngajat);

            if (sendEmail)
            {
                _emailSendService.SendEmailPIPRespinsToAngajat(angajat.Username, angajat.NumePrenume);
                _emailSendService.SendEmailPIPRespinsToManager(manager.Username, manager.NumePrenume, angajat.NumePrenume);

                return Ok(new
                {
                    message = "E-mail-ul a fost trimis la angajat si la managerul direct!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("statusUpdate")]
        public IActionResult SendEmaiPipStatusUpdate([FromQuery] int idAngajat, [FromQuery] int? idFrima)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);
            var useriHr = _userService.GetUseriHR(idFrima);

            if (sendEmail)
            {
                foreach (var userHr in useriHr)
                {
                    _emailSendService.SendEmailPIPUpdateToHr(angajat.NumePrenume, userHr.Username, userHr.NumePrenume);
                }

                _emailSendService.SendEmailPipUpdateToAngajat(angajat.NumePrenume, angajat.Username, angajat.Id);

                return Ok(new
                {
                    message = "Email-ul a fost trimis la departamentul de Resurse Umane si la angajat!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("incheiat")]
        public IActionResult SendEmailPIPIncheiat([FromQuery] int idAngajat, [FromQuery] int? idFrima)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);
            var angajat = _userService.Get(idAngajat);
            var manager = _userService.GetSuperior(idAngajat);
            var useriHr = _userService.GetUseriHR(idFrima);

            if (sendEmail)
            {
                foreach (var userHr in useriHr)
                {
                    _emailSendService.SendEmailPIPIncheiatManagerAndHr(userHr.NumePrenume, userHr.Username, angajat.NumePrenume);
                }
                _emailSendService.SendEmailPIPIncheiatManagerAndHr(manager.NumePrenume, manager.Username, angajat.NumePrenume);
                _emailSendService.SendEmailPIPIncheiatAngajat(angajat.NumePrenume, angajat.Username, angajat.Id);

                return Ok(new
                {
                    message = "Email-ul a fost trimis la departamentul de Resurse Umane, managerul direct si la angajat!",
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
