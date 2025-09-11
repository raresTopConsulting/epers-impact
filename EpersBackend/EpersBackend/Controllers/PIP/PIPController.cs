using Epers.Models;
using Epers.Models.PIP;
using EpersBackend.Services.PIP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.PIP
{
    [Route("api/PIP")]
    [Authorize]
    [ApiController]

    public class PIPController : ControllerBase
    {
        private readonly IPIPService _pipService;

        public PIPController(IPIPService pipService)
        {
            _pipService = pipService;
        }

        [HttpGet("ddSubalterniWithPIP")]
        public IActionResult GetSubalterniWithPIP([FromQuery] string? matricolaSuperior = null)
        {
            return Ok(_pipService.SubalterniThatHavePIP(matricolaSuperior));
        }

        [HttpGet("listaSubalterni/clasificati")]
        public IActionResult GetListaSubalterniClasificati([FromQuery] PaginatedListRequestModel plrqm, [FromQuery] int? anul = null)
        {
            if (plrqm.IdRol == 1 || plrqm.IdRol == 4)
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSublaterniCalaificatiPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSublaterniCalaificatiPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.FilterFirma,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSublaterniCalaificatiPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, null,
                   anul, plrqm.Filter));
                }
            }
            else
                return BadRequest(new { message = "Ne pare rău, nu aveți dreptul să acccesați această listă." }); ;
        }

        [HttpGet("listaSubalterni/aprobare")]
        public IActionResult GetListaSublaterniPtAprobare([FromQuery] PaginatedListRequestModel plrqm, [FromQuery] int? anul = null)
        {
            if (plrqm.IdRol == 1 || plrqm.IdRol == 4)
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniPentruAprobarePip(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniPentruAprobarePip(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.FilterFirma,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSubalterniPentruAprobarePip(plrqm.CurrentPage, plrqm.ItemsPerPage, null,
                   anul, plrqm.Filter));
                }
            }
            else
                return BadRequest(new { message = "Ne pare rău, nu aveți dreptul să acccesați această listă." }); ;
        }

        [HttpGet("listaSubalterni/ongoingPip")]
        public IActionResult GetListaSubalterniOngoingPip([FromQuery] PaginatedListRequestModel plrqm, [FromQuery] int? anul = null)
        {
            if (plrqm.IdRol == 1 || plrqm.IdRol == 4)
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.FilterFirma,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, null,
                   anul, plrqm.Filter));
                }
            }
            else
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSubalterniOngoingPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
            }
        }

        [HttpGet("listaSubalterni/istoricPip")]
        public IActionResult GetListaSubalterniIstoricPip([FromQuery] PaginatedListRequestModel plrqm, [FromQuery] int? anul = null)
        {
            if (plrqm.IdRol == 1 || plrqm.IdRol == 4)
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.FilterFirma,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPipForAdmin(plrqm.CurrentPage, plrqm.ItemsPerPage, null,
                   anul, plrqm.Filter));
                }
            }
            else
            {
                if (plrqm.IdFirmaLoggedInUser.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                if (plrqm.FilterFirma.HasValue)
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
                else
                {
                    return Ok(_pipService.GetListaSubalterniIstoricPip(plrqm.MatricolaLoggedInUser, plrqm.CurrentPage, plrqm.ItemsPerPage, plrqm.IdFirmaLoggedInUser,
                    anul, plrqm.Filter));
                }
            }
        }

        [HttpGet("createInitial/{idAngajat}")]
        public IActionResult CreateInitial(int idAngajat)
        {
            return Ok(_pipService.CreateInitial(idAngajat));
        }

        [HttpGet("display")]
        public IActionResult GetPipDisplayModel([FromQuery] int idAngajat, [FromQuery] int? year = null)
        {
            return Ok(_pipService.Get(idAngajat, year));
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] PipDisplayAddEditModel pipDisplayAddEditModel)
        {
            if (ModelState.IsValid)
            {
                _pipService.Add(pipDisplayAddEditModel);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("update")]
        public IActionResult Update([FromBody] PipDisplayAddEditModel pipDisplayAddEditModel)
        {
            if (ModelState.IsValid)
            {
                _pipService.Update(pipDisplayAddEditModel);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("actualizare/utilizatoriNeedPip/{anul}")]
        public IActionResult ActualizareUtilizatoriNeedPip(int anul)
        {
            var result = _pipService.ActualizareListaSublaterniThatNeedPip(anul);

            return Ok(new { message = result });
        }

        [HttpGet("hasMediaForPip")]
        public IActionResult HasMediaForPip([FromQuery] int idAngajat, [FromQuery] int? year = null)
        {
            return Ok(_pipService.AngajatAreMediaPtPip(idAngajat, year));
        }

        [HttpGet("stareActuala")]
        public IActionResult GetStareActualaPip([FromQuery] int idAngajat, [FromQuery] int? year = null)
        {
            return Ok(_pipService.GetStareActualaPip(idAngajat, year));
        }

    }
}

