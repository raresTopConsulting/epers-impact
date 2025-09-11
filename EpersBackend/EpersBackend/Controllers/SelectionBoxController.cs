using EpersBackend.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EpersBackend.Controllers.Nomenclatoare
{
    [Authorize]
    [Route("api/[controller]")]

    public class SelectionBoxController : ControllerBase
    {
        private readonly IEfSelectionBoxRepository _efSelectionBoxRepo;

        public SelectionBoxController(IEfSelectionBoxRepository efSelectionBoxRepo)
        {
            _efSelectionBoxRepo = efSelectionBoxRepo;
        }

        [HttpGet]
        public IActionResult AllSelections()
        {
            var selections = _efSelectionBoxRepo.GetSelections();
            if (selections == null)
            {
                return NotFound();
            }
            return Ok(JsonConvert.SerializeObject(selections));
        }

        [Route("judete")]
        [HttpGet]
        public IActionResult GetJudete()
        {
            var judete = _efSelectionBoxRepo.GetJudete();

            if (judete.Count > 0)
                return Ok(judete);
            else
                return BadRequest();
        }

        [Route("competente")]
        [HttpGet]
        public IActionResult GetCompetente()
        {
            var tipuriComp = _efSelectionBoxRepo.GetTipuriCompetente();

            if (tipuriComp.Count > 0)
                return Ok(tipuriComp);
            else
                return BadRequest();
        }

        [Route("frecventaObiective")]
        [HttpGet]
        public IActionResult GetFrecventaObiective()
        {
            var frecventeOb = _efSelectionBoxRepo.GetFrecventeObiective();
            return Ok(frecventeOb);
        }

        [Route("tipObiective")]
        [HttpGet]
        public IActionResult GetTipObiective()
        {
            var tipuriOb = _efSelectionBoxRepo.GetTipuriObiective();
            return Ok(tipuriOb);
        }
    }
}

