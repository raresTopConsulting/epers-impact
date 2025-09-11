using EpersBackend.Services.PDF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Nomenclatoare
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class PDFController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PDFController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [Route("evaluare")]
        [HttpGet]
        public IActionResult GetPdfEvaluare(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            var pdfBytes = _pdfService.GeneratePDFEvaluare(idAngajat, anul);
            var base64String = Convert.ToBase64String(pdfBytes);

            return Ok(new
            {
                fileContents = base64String,
                contentType = "application/pdf",
                fileDownloadName = "Evaluare + " + idAngajat + " anul " + anul + ".pdf"
            });
        }

        [Route("evaluare/concluzii")]
        [HttpGet]
        public IActionResult GetPdfEvaluareConcluzii(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            var pdfBytes = _pdfService.GeneratePDFEvaluareSiConcluzii(idAngajat, anul);
            var base64String = Convert.ToBase64String(pdfBytes);

            return Ok(new
            {
                fileContents = base64String,
                contentType = "application/pdf",
                fileDownloadName = "Evaluare + " + idAngajat + " anul " + anul + ".pdf"
            });
        }

        [Route("obiective/actuale")]
        [HttpGet]
        public IActionResult GetPdfObiectiveActuale(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            var pdfBytes = _pdfService.GeneratePDFObiectiveActuale(idAngajat, anul);
            var base64String = Convert.ToBase64String(pdfBytes);

            return Ok(new
            {
                fileContents = base64String,
                contentType = "application/pdf",
                fileDownloadName = "Obiective + " + idAngajat + " anul " + anul + ".pdf"
            });
        }

        [Route("obiective/istoric")]
        [HttpGet]
        public IActionResult GetPdfObiectiveIstoric(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            var pdfBytes = _pdfService.GeneratePDFObiectiveIstoric(idAngajat, anul);
            var base64String = Convert.ToBase64String(pdfBytes);

            return Ok(new
            {
                fileContents = base64String,
                contentType = "application/pdf",
                fileDownloadName = "Obiective + " + idAngajat + " anul " + anul + ".pdf"
            });
        }

        [Route("PIP")]
        [HttpGet]
        public IActionResult GetPdfPIP(int idAngajat, int? anul = null)
        {
            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }

            var pdfBytes = _pdfService.GeneratePDFPip(idAngajat, anul);
            var base64String = Convert.ToBase64String(pdfBytes);

            return Ok(new
            {
                fileContents = base64String,
                contentType = "application/pdf",
                fileDownloadName = "PIP + " + idAngajat + " anul " + anul + ".pdf"
            });
        }

    }
}

