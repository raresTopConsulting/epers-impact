using EpersBackend.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Email
{
    [Route("api/EmailEvaluare")]
    [Authorize]
    [ApiController]

    public class EmailEvaluareController : ControllerBase
    {
        private readonly IEmailSendService _emailSendService;
        private readonly IConfiguration _configuration;

        public EmailEvaluareController(IEmailSendService emailSendService,
            IConfiguration configuration)
        {
            _emailSendService = emailSendService;
            _configuration = configuration;
        }

        [HttpGet("autoevaluare")]
        public IActionResult SendEmailAutoevaluare([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);

            if (sendEmail)
            {
                _emailSendService.SendEmailAutoevaluare(idAngajat);

                return Ok(new
                {
                    message = "O notificare prin email pentru evaluare a fost trimisă!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("evaluareSublatern")]
        public IActionResult SendEmailEvaluareSubaltern([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);

            if (sendEmail)
            {
                _emailSendService.SendEmailEvaluareSubaltern(idAngajat);

                return Ok(new
                {
                    message = "O notificare prin email pentru evaluare a fost trimisă!",
                    emailSent = true
                });
            }
            return Ok(new
            {
                message = "Trimiterea de email-uri nu este activata! Daca doriti ca aceasta sa fie activata, contactați administratorul aplicației!",
                emaiSent = false
            });
        }

        [HttpGet("evaluareFinalaSubaltern")]
        public IActionResult SendEmailEvaluareFinalaSubaltern([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);

            if (sendEmail)
            {
                _emailSendService.SendEmailEvaluareFinalaSubaltern(idAngajat);

                return Ok(new
                {
                    message = "O notificare prin email pentru evaluare a fost trimisă!",
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