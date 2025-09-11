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

    public class DiviziiController : ControllerBase
    {
        private readonly IEfDiviziiRepository _efDiviziiRepo;

        public DiviziiController(IEfDiviziiRepository efDiviziiRepo)
        {
            _efDiviziiRepo = efDiviziiRepo;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efDiviziiRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efDiviziiRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var divizie = _efDiviziiRepo.Get(id);

            if (divizie != null)
                return Ok(divizie);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NDivizii divizie)
        {
            if (ModelState.IsValid)
            {
                _efDiviziiRepo.Add(divizie);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut()]
        public IActionResult Put([FromBody]NDivizii divizie)
        {
            if (ModelState.IsValid)
            {
                _efDiviziiRepo.Update(divizie);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efDiviziiRepo.SetToInactive(id);
            return Ok(true);
        }
    }
}

