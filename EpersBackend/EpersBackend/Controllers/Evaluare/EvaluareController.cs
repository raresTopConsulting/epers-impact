using Epers.Models;
using Epers.Models.Evaluare;
using EpersBackend.Services.Evaluare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Evaluare
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EvaluareController : ControllerBase
    {
        private readonly IEvaluareService _evaluareService;
        private readonly INotiteService _notiteService;

        public EvaluareController(IEvaluareService evaluareService,
            INotiteService notiteService)
        {
            _evaluareService = evaluareService;
            _notiteService = notiteService;
        }

        [HttpGet("listaSubalterni")]
        public IActionResult GetListaSubalterni([FromQuery] PaginatedListRequestModel plrqm)
        {
            // admin or hr all firme
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_evaluareService.GetListaAngajatiAdminHrAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.FilterFirma));
            }

            // admin or hr firma 
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_evaluareService.GetListaAngajatiAdminHrFirmaPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.Filter));
            }

            // normal user firma user
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_evaluareService.GetListaSubalterniPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.MatricolaLoggedInUser, plrqm.Filter));
            }

            // normal user all firme -> maybe not used
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_evaluareService.GetListaSubalterniAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.MatricolaLoggedInUser, plrqm.FilterFirma));
            }

            return BadRequest("Cere invalida");
        }

        [HttpGet("afisareSkillsEval")]
        public IActionResult GetAfisareSkills([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_evaluareService.GetEvalTemplate(idAngajat, anul));
        }

        [HttpGet("contestareEvaluare")]
        public IActionResult ContestareEvaluare([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            _evaluareService.ContestareEvaluare(idAngajat, anul);
            return Ok();
        }

        [HttpGet("mentiuni")]
        public IActionResult GetMentiuni([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_notiteService.GetNotiteUser(idAngajat, anul));
        }

        [HttpGet("istoric")]
        public IActionResult GetIstoric([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_evaluareService.GetIstoricEvalTemplate(idAngajat, anul));
        }

        [HttpGet("istoric/calificativFinal")]
        public IActionResult GetIstoricEvalCalificativFinal([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_evaluareService.GetIstoricEvalCalificativFinal(idAngajat, anul));
        }

        [HttpPost("mentiuni")]
        public IActionResult AddMentiune([FromBody] Notite mentiuni)
        {
            if (ModelState.IsValid)
            {
                _notiteService.Add(mentiuni);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("upsertEvaluare")]
        public IActionResult UpsertEvaluare([FromBody] EvaluareTemplate evaluare)
        {
            if (ModelState.IsValid)
            {
                _evaluareService.UpsertEvaluare(evaluare);
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
