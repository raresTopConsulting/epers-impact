using Epers.Models;
using Epers.Models.Concluzii;
using EpersBackend.Services.Evaluare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Evaluare
{
    [Route("api/Concluzii")]
    [Authorize]
    [ApiController]

    public class ConcluziiControler : ControllerBase
    {
        private readonly IConcluziiService _concluziiService;

        public ConcluziiControler(IConcluziiService concluziiService)
        {
            _concluziiService = concluziiService;
        }

        [HttpGet("listaSubalterni")]
        public IActionResult GetListaSubalterni([FromQuery] PaginatedListRequestModel plrqm)
        {
            // admin or hr all firme
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_concluziiService.GetListaAngajatiAdminHrAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.FilterFirma));
            }

            // admin or hr firma 
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_concluziiService.GetListaAngajatiAdminHrFirmaPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.Filter));
            }

            // normal user firma user
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_concluziiService.GetListaSubalterniPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.MatricolaLoggedInUser, plrqm.Filter));
            }

            // normal user all firme -> maybe not used
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_concluziiService.GetListaSubalterniAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.MatricolaLoggedInUser, plrqm.FilterFirma));
            }

            return BadRequest("Cere invalida");
        }

        [HttpGet("idsEvaluariSubaltern")]
        public IActionResult GetIdsEvaluariSubaltern([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_concluziiService.GetIdsEvaluariSubaltern(idAngajat, anul));
        }

        [HttpGet("istoric")]
        public IActionResult GetIstoric([FromQuery] int idAngajat, [FromQuery] int? anul = null)
        {
            return Ok(_concluziiService.GetIstoric(idAngajat, anul));
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] Concluzie concluzie,
            [FromQuery] long[] idEvaluari, [FromQuery] string loggedInUserMatricola)
        {
            if (ModelState.IsValid)
            {
                _concluziiService.Add(concluzie, idEvaluari, loggedInUserMatricola);
                return Ok();
            }

            return BadRequest();
        }
    }
}

