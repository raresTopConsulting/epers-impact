using EpersBackend.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Email
{
    [Route("api/EmailConcluzii")]
    [Authorize]
    [ApiController]

    public class EmailConcluziiController : ControllerBase
    {
        private readonly IEmailSendService _emailSendService;
        private readonly IConfiguration _configuration;

        public EmailConcluziiController(IConfiguration configuration,
            IEmailSendService emailSendService)
        {
            _configuration = configuration;
            _emailSendService = emailSendService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult SendEmailObiectiveSetate([FromQuery] int idAngajat)
        {
            var sendEmail = bool.Parse(_configuration["EmailSettings:Enabled"]);

            if (sendEmail)
            {
                _emailSendService.SenEmailConcluziiEvaluare(idAngajat);

                return Ok(new
                {
                    message = "Email-ul a fost trimis cu success!",
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