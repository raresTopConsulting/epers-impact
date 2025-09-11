using EpersBackend.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Email
{
    [Route("api/EmailPassword")]
    [Authorize]
    [ApiController]

    public class EmailPasswordController : ControllerBase
    {
        private readonly IEmailSendService _emailSendService;

        public EmailPasswordController(IEmailSendService emailSendService)
        {
            _emailSendService = emailSendService;
        }

    }
}