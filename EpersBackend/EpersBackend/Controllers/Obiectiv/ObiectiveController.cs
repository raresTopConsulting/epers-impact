using Epers.Models;
using Epers.Models.Obiectiv;
using Epers.Models.Users;
using EpersBackend.Services.Nomenclatoare;
using EpersBackend.Services.ObiectivService;
using EpersBackend.Services.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Obiectiv
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class ObiectiveController : ControllerBase
    {
        private readonly IObiectiveService _obiectiveService;
        private readonly IEfNObiectiveRepository _efNObiectiveRepository;
        private readonly IPagination _paginationService;

        public ObiectiveController(IObiectiveService obiectiveService,
            IPagination paginationService, IEfNObiectiveRepository efNObiectiveRepository)
        {
            _obiectiveService = obiectiveService;
            _paginationService = paginationService;
            _efNObiectiveRepository = efNObiectiveRepository;
        }

        [HttpGet("nomObiectiv/{id}")]
        public IActionResult GetObiectiv(int id)
        {
            return Ok(_efNObiectiveRepository.Get(id));
        }

        [HttpGet("listaSubalterni")]
        public IActionResult GetListaSubalterni([FromQuery] PaginatedListRequestModel plrqm)
        {
            // admin or hr all firme
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_obiectiveService.GetListaAngajatiAdminHrAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.FilterFirma));
            }

            // admin or hr firma 
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol == 1 || plrqm.IdRol == 4))
            {
                return Ok(_obiectiveService.GetListaAngajatiAdminHrFirmaPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.Filter));
            }

            // normal user firma user
            if (plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_obiectiveService.GetListaSubalterniPaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser.Value, plrqm.MatricolaLoggedInUser, plrqm.Filter));
            }

            // normal user all firme -> maybe not used
            if (!plrqm.IdFirmaLoggedInUser.HasValue && (plrqm.IdRol != 1 || plrqm.IdRol != 4))
            {
                return Ok(_obiectiveService.GetListaSubalterniAllFirmePaginated(plrqm.CurrentPage,
                    plrqm.ItemsPerPage, plrqm.Filter, plrqm.MatricolaLoggedInUser, plrqm.FilterFirma));
            }

            return BadRequest("Cere invalida");
        }

        [HttpGet("actuale")]
        public IActionResult GetObiectiveActuale([FromQuery] int idAngajat,
            [FromQuery] int currentPage, [FromQuery] int itemsPerPage, [FromQuery] string? filter = null)
        {
            Obiective[] listaObActuale = Array.Empty<Obiective>();

            if (filter != null)
                listaObActuale = _obiectiveService.GetObiectiveActuale(idAngajat, filter);
            else
                listaObActuale = _obiectiveService.GetObiectiveActuale(idAngajat, null);

            var items = listaObActuale.Length;
            var pageSettings = _paginationService.GetPages(currentPage, items, itemsPerPage);

            var obActualeDispaly = new ObiectiveDisplayModel
            {
                ListaObActuale = listaObActuale.Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems).ToArray(),
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };

            return Ok(obActualeDispaly);
        }

        [HttpGet("istoric")]
        public IActionResult GetIstoricObiective([FromQuery] int idAngajat,
            [FromQuery] int currentPage, [FromQuery] int itemsPerPage, [FromQuery] string? filter = null)
        {
            Obiective[] lsitaIstoricOb = Array.Empty<Obiective>();

            if (filter != null)
                lsitaIstoricOb = _obiectiveService.GetIstoricObiective(idAngajat, filter);
            else
                lsitaIstoricOb = _obiectiveService.GetIstoricObiective(idAngajat, null);

            var items = lsitaIstoricOb.Length;
            var pageSettings = _paginationService.GetPages(currentPage, items, itemsPerPage);

            var obActualeDispaly = new ObiectiveDisplayModel
            {
                ListaObActuale = lsitaIstoricOb.Skip(pageSettings.ItemBeginIndex)
                    .Take(pageSettings.DisplayedItems).ToArray(),
                CurrentPage = currentPage,
                Pages = pageSettings.Pages
            };

            return Ok(obActualeDispaly);
        }

        [HttpPost("setareObiective")]
        public IActionResult SetareObiective([FromBody] SetareObiective setareObiective,
            [FromQuery] int[]? idAngajatiSelectati = null)
        {
            if (ModelState.IsValid)
            {
                _obiectiveService.Add(setareObiective, idAngajatiSelectati);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("evaluareObiective")]
        public IActionResult EvaluareObiective([FromBody] Obiective[] evalObiective)
        {
            if (ModelState.IsValid)
            {
                _obiectiveService.Evaluare(evalObiective);
                return Ok();
            }
            return BadRequest();
        }

    }
}

