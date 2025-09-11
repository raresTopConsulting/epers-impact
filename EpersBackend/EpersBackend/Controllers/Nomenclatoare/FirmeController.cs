using Epers.Models;
using Epers.Models.Nomenclatoare;
using EpersBackend.Services.Nomenclatoare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace EpersBackend.Controllers.Nomenclatoare
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class FirmeController : ControllerBase
    {
        private readonly IEfFirmeRepository _efFirmeRepo;


        public FirmeController(IEfFirmeRepository efFrimeRepo)
        {
            _efFirmeRepo = efFrimeRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            NFirme[] firme = Array.Empty<NFirme>();

            firme = _efFirmeRepo.GetAll();

            if (firme.Length > 0)
            {
                return Ok(firme);
            }
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("dropdown")]
        public IActionResult GetDDFirme()
        {
            var fime = _efFirmeRepo.GetAll();

            var firmeSeleciton = fime.Select(frm => new DropdownSelection
            {
                Id = frm.Id,
                Text = frm.Denumire.Trim(),
                Value = frm.Id.ToString(),
                IdFirma = frm.Id
            }).ToList();

            return Ok(JsonConvert.SerializeObject(firmeSeleciton));
        }

        [HttpGet]
        [Route("paginated")]
        public IActionResult Get([FromQuery] int currentPage, [FromQuery] int itemsPerPage,
            [FromQuery] string? filter = null)
        {
            return Ok(_efFirmeRepo.GetAllPaginated(currentPage, itemsPerPage, filter));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var firma = _efFirmeRepo.Get(id);

            if (firma != null)
                return Ok(firma);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] NFirme firma)
        {
            if (ModelState.IsValid)
            {
                _efFirmeRepo.Add(firma);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] NFirme firma)
        {
            if (ModelState.IsValid)
            {
                _efFirmeRepo.Update(firma);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _efFirmeRepo.SetToInactive(id);
            return Ok(true);
        }
    }
}

