using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Scripts
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]

    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Aplicatia functioneaza!");
        }
    }
}
