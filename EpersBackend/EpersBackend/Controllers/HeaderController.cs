using EpersBackend.Services.Header;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class HeaderController : ControllerBase
    {
        private readonly IHeaderService _headerService;

        public HeaderController(IHeaderService headerService)
        {
            _headerService = headerService;
        }

        [HttpGet("{idAngajat}")]
        public IActionResult Get(int idAngajat)
        {
            return Ok(_headerService.GetHeader(idAngajat));
        }

        [HttpGet("userDetails/{id}")]
        public IActionResult GetUserDetails(int id)
        {
            return Ok(_headerService.GetUserDetails(id));
        }
    }
}

