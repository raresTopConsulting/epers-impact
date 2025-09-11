using Epers.Models;
using Epers.Models.Nomenclatoare;
using EpersBackend.Services.Nomenclatoare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Nomenclatoare
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PosturiController : ControllerBase
    {
        private readonly IEfPosturiRepository _efPosturiRepo;
        private readonly IEfSetareProfilPostRepository _efSetareProfilRepo;

        public PosturiController(IEfPosturiRepository efPosturiRepo,
            IEfSetareProfilPostRepository efSetareProfilRepo)
        {
            _efPosturiRepo = efPosturiRepo;
            _efSetareProfilRepo = efSetareProfilRepo;
        }

        [HttpGet]
        public IActionResult GetAllPaginated([FromQuery] PaginatedListRequestModel paginatedListRequestModel)
        {
            if (!paginatedListRequestModel.IdFirmaLoggedInUser.HasValue && (paginatedListRequestModel.IdRol == 1 || paginatedListRequestModel.IdRol == 4))
            {
                return Ok(_efPosturiRepo.GetAllForAllFirmePaginated(paginatedListRequestModel.CurrentPage, paginatedListRequestModel.ItemsPerPage,
                    paginatedListRequestModel.Filter, paginatedListRequestModel.FilterFirma));
            }
            return Ok(_efPosturiRepo.GetAllPaginated(paginatedListRequestModel.CurrentPage, paginatedListRequestModel.ItemsPerPage,
                paginatedListRequestModel.IdFirmaLoggedInUser.Value, paginatedListRequestModel.Filter));
        }

        [HttpGet("{id}")]
        public IActionResult GetPost(int id)
        {
            var post = _efPosturiRepo.Get(id);

            if (post != null)
                return Ok(post);
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult AddPost([FromBody] NPosturi post)
        {
            if (ModelState.IsValid)
            {
                _efPosturiRepo.Add(post);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdatePost([FromBody]NPosturi post)
        {
            if (ModelState.IsValid)
            {
                _efPosturiRepo.Update(post);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            _efPosturiRepo.SetToInactive(id);
            return Ok(true);
        }

        [HttpPost("addProfilPost")]
        public IActionResult AddProfilPost([FromBody] SetareProfil setareProfil)
        {
            if (ModelState.IsValid)
            {
                _efSetareProfilRepo.Add(setareProfil);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("updateProfilPost")]
        public IActionResult UpdateProfilPost([FromBody] SetareProfil setareProfil)
        {
            if (ModelState.IsValid)
            {
                _efSetareProfilRepo.Update(setareProfil);
                return Ok(true);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("getProfilPost/{idPost}")]
        public IActionResult GetProfilPost(int idPost)
        {
            var profilPost = _efSetareProfilRepo.Get(idPost);

            return Ok(profilPost);
        }

        [HttpDelete("deleteProfiliPost/{idProfilPost}")]
        public IActionResult DeleteProfilPost(int idProfilPost)
        {
            _efSetareProfilRepo.Delete(idProfilPost);

            return Ok(true);
        }

    }
}

