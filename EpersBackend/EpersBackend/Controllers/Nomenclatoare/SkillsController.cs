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

    public class SkillsController : ControllerBase
    {
        private readonly IEfSkillsRepository _efSkillsRepo;

        public SkillsController(IEfSkillsRepository efSkillsRepo)
        {
            _efSkillsRepo = efSkillsRepo;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efSkillsRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage,
                 paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efSkillsRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage,
                paginatedListRequestModel.ItemsPerPage, paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var skill = _efSkillsRepo.Get(id);

            if (skill != null)
                return Ok(skill);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NSkills skill)
        {
            if (ModelState.IsValid)
            {
                _efSkillsRepo.Add(skill);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] NSkills skill)
        {
            if (ModelState.IsValid)
            {
                _efSkillsRepo.Update(skill);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efSkillsRepo.SetToInactive(id);
            return Ok(true);
        }
    }
}

