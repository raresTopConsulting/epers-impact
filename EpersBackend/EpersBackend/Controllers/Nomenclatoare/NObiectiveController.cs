using Epers.Models;
using Epers.Models.Nomenclatoare;
using EpersBackend.Services.Nomenclatoare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Nomenclatoare
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class NObiectiveController : ControllerBase
    {
        private readonly IEfNObiectiveRepository _efNObiectRepo;

        public NObiectiveController(IEfNObiectiveRepository efDiviziiRepo)
        {
            _efNObiectRepo = efDiviziiRepo;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efNObiectRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efNObiectRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var divizie = _efNObiectRepo.Get(id);

            if (divizie != null)
                return Ok(divizie);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NObiective obiectiv)
        {
            if (ModelState.IsValid)
            {
                _efNObiectRepo.Add(obiectiv);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] NObiective obiectiv)
        {
            if (ModelState.IsValid)
            {
                _efNObiectRepo.Update(obiectiv);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

    }
}

