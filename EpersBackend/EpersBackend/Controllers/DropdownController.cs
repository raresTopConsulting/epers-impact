using Epers.Models;
using Epers.Models.Users;
using EpersBackend.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EpersBackend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class DropdownController : ControllerBase
    {
        private readonly IDrodpwonRepository _ddRepository;

        public DropdownController(IDrodpwonRepository drodpwonRepository)
        {
            _ddRepository = drodpwonRepository;
        }

        [HttpGet]
        public IActionResult AllDropdowns()
        {
            var dropdowns = _ddRepository.GetAllDropdownSelections();
            
            return Ok(JsonConvert.SerializeObject(dropdowns));
        }

        [HttpGet("nomenclatoare")]
        public NomenclatoareSelection Nomenclatoare()
        {
            return _ddRepository.GetNomenclatoareSelection();
        }

        [HttpGet("roluri")]
        public List<DropdownSelection> Roluri()
        {
            return _ddRepository.GetDDRoluri();
        }

        [HttpGet("useri")]
        public List<DropdownSelection> Useri()
        {
            return _ddRepository.GetDDUseri();
        }

        [HttpGet("divizii")]
        public List<DropdownSelection> Divizii()
        {
            return _ddRepository.GetDDDivizii();
        }

        //[HttpGet("skills")]
        //public List<DropdownSelection> Skills()
        //{
        //    return _ddRepository.GetDDCompetente();
        //}


        [HttpGet("nomenclatorObiective")]
        public List<DropdownSelection> GetNomenclatorObiective()
        {
            return _ddRepository.GetDDObiective();
        }

    }
}

