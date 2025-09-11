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


    public class CursuriController : ControllerBase
    {
        private readonly IEfCursuriRepository _efCursuriRepository;

        public CursuriController(IEfCursuriRepository efCursuriRepo)
        {
            _efCursuriRepository = efCursuriRepo;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efCursuriRepository.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efCursuriRepository.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var compartiment = _efCursuriRepository.Get(id);

            if (compartiment != null)
                return Ok(compartiment);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NCursuri curs)
        {
            if (ModelState.IsValid)
            {
                _efCursuriRepository.Add(curs);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut()]
        public IActionResult Put([FromBody] NCursuri curs)
        {
            if (ModelState.IsValid)
            {
                _efCursuriRepository.Update(curs);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efCursuriRepository.SetToInactive(id);
            return Ok(true);
        }
    
    }
}

