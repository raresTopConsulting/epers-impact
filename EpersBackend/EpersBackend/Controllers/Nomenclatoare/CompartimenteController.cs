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

    public class CompartimenteController : ControllerBase
    {
        private readonly IEfCompartimenteRepository _efCompartimenteRepo;

        public CompartimenteController(IEfCompartimenteRepository efCompartimenteRepo)
        {
            _efCompartimenteRepo = efCompartimenteRepo;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efCompartimenteRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efCompartimenteRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var compartiment = _efCompartimenteRepo.Get(id);

            if (compartiment != null)
                return Ok(compartiment);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NCompartimente compartiment)
        {
            if (ModelState.IsValid)
            {
                _efCompartimenteRepo.Add(compartiment);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut()]
        public IActionResult Put([FromBody] NCompartimente compartiment)
        {
            if (ModelState.IsValid)
            {
                _efCompartimenteRepo.Update(compartiment);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efCompartimenteRepo.SetToInactive(id);
            return Ok(true);
        }

    }
}

