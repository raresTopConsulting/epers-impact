using Epers.Models.Excel;
using EpersBackend.Services.Evaluare;
using EpersBackend.Services.Excel;
using EpersBackend.Services.PDF;
using EpersBackend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpersBackend.Controllers.Scripts
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class ExportEvaluariController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPdfService _pdfService;
        private readonly IPdfFileOperationService _pdfFileOperationService;
        private readonly IConfiguration _configuration;
        private readonly IExcelRapoarteService _excelRapoarteService;
        private readonly IEvaluareService _evaluareService;

        public ExportEvaluariController(IUserService userService,
            IPdfFileOperationService pdfFileOperationService,
            IPdfService pdfService,
            IExcelRapoarteService excelRapoarteService,
            IConfiguration configuration,
            IEvaluareService evaluareService)
        {
            _userService = userService;
            _pdfService = pdfService;
            _pdfFileOperationService = pdfFileOperationService;
            _configuration = configuration;
            _excelRapoarteService = excelRapoarteService;
            _evaluareService = evaluareService;
        }


        [HttpGet("all")]
        public IActionResult ExportEvaluariToExcel([FromQuery] int? anul = null)
        {
            var excelRaportEvaluariList = new List<ExcelRaportEvaluari>();

            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }
            var usersList = _userService.GetAll();

            foreach (var user in usersList)
            {
                if (_evaluareService.UserHasEvaluareFinalizata(user.Id, anul.Value))
                {
                    var urlSavedPdf = PathSavedPdfEvaluareSiConcluziiToFolder(user.Id, anul.Value);

                    excelRaportEvaluariList.Add(new ExcelRaportEvaluari
                    {
                        IdAngajat = user.Id,
                        NumeSiPrenume = user.NumePrenume,
                        Marca = user.Matricola,
                        SesiuneEvaluare = anul.Value,
                        Url = urlSavedPdf
                    });
                }
            }

            string excelSaveSuccess = _excelRapoarteService.SaveExcelRapoarte(excelRaportEvaluariList.ToArray());

            return Ok(new { raspuns = excelSaveSuccess });
        }


        [HttpGet("between")]
        public IActionResult ExportEvaluariToExcelForAngajatiBetween([FromQuery] int idStart, [FromQuery] int idStop,
            [FromQuery] int? anul = null)
        {
            var excelRaportEvaluariList = new List<ExcelRaportEvaluari>();

            if (!anul.HasValue)
            {
                anul = DateTime.Now.Year;
            }
            var usersList = _userService.GetAllBetween(idStart, idStop);

            foreach (var user in usersList)
            {
                if (_evaluareService.UserHasEvaluareFinalizata(user.Id, anul.Value))
                {
                    var urlSavedPdf = PathSavedPdfEvaluareSiConcluziiToFolder(user.Id, anul.Value);

                    excelRaportEvaluariList.Add(new ExcelRaportEvaluari
                    {
                        IdAngajat = user.Id,
                        NumeSiPrenume = user.NumePrenume,
                        Marca = user.Matricola,
                        SesiuneEvaluare = anul.Value,
                        Url = urlSavedPdf
                    });
                }
            }

            string excelSaveSuccess = _excelRapoarteService.SaveExcelRapoarte(excelRaportEvaluariList.ToArray());

            return Ok(new { raspuns = excelSaveSuccess });
        }

        // [HttpGet("PDF/EvaluareSiConcluzii")]
        // public IActionResult SavePdfEvaluareToFolder(int idAngajat, int? anul = null)
        // {
        //     if (!anul.HasValue)
        //     {
        //         anul = DateTime.Now.Year;
        //     }

        //     var numePdf = PathSavedPdfEvaluareSiConcluziiToFolder(idAngajat, anul.Value);

        //     return Ok(numePdf);
        // }

        private string PathSavedPdfEvaluareSiConcluziiToFolder(int idAngajat, int anul)
        {
            var angajat = _userService.Get(idAngajat);
            var pdfBytes = _pdfService.GeneratePDFEvaluareSiConcluzii(idAngajat, anul);
            var numePdf = $"ConcluziiEvaluare_{angajat.NumePrenume}_{angajat.Matricola}_{anul}";
            var savedRaportPath = _configuration["PDF:FileSavePath"];
            savedRaportPath += $"/{numePdf}.pdf";

            _pdfFileOperationService.SavePdfToFolder(pdfBytes, savedRaportPath);

            return savedRaportPath;
        }

    }
}
