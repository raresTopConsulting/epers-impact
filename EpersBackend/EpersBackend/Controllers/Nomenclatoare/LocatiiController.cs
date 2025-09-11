using Epers.Models;
using Epers.Models.Nomenclatoare;
using EpersBackend.Services.Nomenclatoare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Nomenclatoare
{
    //[RoutePrefix("api/Locatii")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class LocatiiController : ControllerBase
    {
        private readonly IEfLocatiiRepository _efLocatiiRepo;

        public LocatiiController(IEfLocatiiRepository efLocatiiRepo)
        {
            _efLocatiiRepo = efLocatiiRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            NLocatii[] locatii = Array.Empty<NLocatii>();

            locatii = _efLocatiiRepo.GetAll();

            if (locatii.Length > 0)
            {
                return Ok(locatii);
            }
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("paginated")]

        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efLocatiiRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efLocatiiRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var locatie = _efLocatiiRepo.Get(id);

            if (locatie != null)
                return Ok(locatie);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NLocatii locatie)
        {
            if (ModelState.IsValid)
            {
                _efLocatiiRepo.Add(locatie);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] NLocatii locatie)
        {
            if (ModelState.IsValid)
            {
                _efLocatiiRepo.Update(locatie);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efLocatiiRepo.SetToInactive(id);
            return Ok(true);
        }
    }
}

