using Epers.Models;
using Epers.Models.Users;
using EpersBackend.Services.Sincron;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISincronizareSincron _sincronService;

        public UserController(IUserService userService,
            ISincronizareSincron sicronService)
        {
            _userService = userService;
            _sincronService = sicronService;
        }

        [HttpGet("listaUtilizatori")]
        public IActionResult GetListaUtilizatori([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_userService.GetListaUtilizatoriAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_userService.GetListaUtilizatoriPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.Get(id);

            return Ok(user);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCreateModel userCreate)
        {
            if (ModelState.IsValid)
            {
                _userService.Register(userCreate);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("updateUserData")]
        public IActionResult UpdateUserData([FromBody] UserEditModel userEdit)
        {
            if (ModelState.IsValid)
            {
                _userService.Update(userEdit);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet("registerSincron")]
        public IActionResult RegisterFromSincron([FromQuery] int minId, [FromQuery] int maxId)
        {
            for (int id = minId; id <= maxId; id++)
            {
                _sincronService.AddSincronEmployeeToEpers(id);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userService.Delete(id);

            return Ok();
        }

        [HttpGet("dropdownSublaterni")]
        public IActionResult GetDDSublaterni([FromQuery] int userRolId, [FromQuery] string loggedInUserMatricola)
        {
            if (userRolId == 1)
            {
                return Ok(_userService.GetListaAngajatiForAdminDropdown());
            }
            return Ok(_userService.GetListaSubalterniDropdown(loggedInUserMatricola));
        }


        //[HttpGet("username")]
        //public ActionResult<string> GetLoggedInUser()
        //{
        //    var userName = GetLoggedInUsername();
        //    return Ok(userName);
        //}


        //private string GetLoggedInUsername()
        //{
        //    var result = string.Empty;

        //    if (_httpContextAccessor.HttpContext != null)
        //    {
        //        result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        //    }

        //    return result != null ? result : string.Empty;
        //}


    }
}

