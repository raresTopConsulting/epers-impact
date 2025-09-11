using Epers.Models.Nomenclatoare;
using Epers.Models.Pdf;
using EpersBackend.Services.Evaluare;
using EpersBackend.Services.Header;
using EpersBackend.Services.Nomenclatoare;

namespace EpersBackend.Services.PDF
{
    public class PdfEvalareDocumentsConclzuieDataSource: IPdfEvalareDocumentsConclzuieDataSource
    {
        private readonly IEvaluareService _evaluareService;
        private readonly IHeaderService _headerService;
        private readonly IConcluziiService _concluzieService;
        private readonly IEfCursuriRepository _efCursuriRepository;

        public PdfEvalareDocumentsConclzuieDataSource(IEvaluareService evaluareService,
            IHeaderService headerService,
            IConcluziiService concluzieService,
            IEfCursuriRepository efCursuriRepository)
        {
            _evaluareService = evaluareService;
            _headerService = headerService;
            _concluzieService = concluzieService;
            _efCursuriRepository = efCursuriRepository;
        }

        public PdfEvaluareConcluziiModel GetPdfEvaluareSiConcluziiData(int idAngajat, int? anul = null)
        {
            var headerData = _headerService.GetHeader(idAngajat);
            var dateEvaluare = _evaluareService.GetIstoricEvalTemplate(idAngajat, anul);
            var concluziiEvaluare =  _concluzieService.GetIstoric(idAngajat, anul);
            var listaTraininguri = new List<NCursuri>();

            if (concluziiEvaluare != null && concluziiEvaluare.IdTraining != null && concluziiEvaluare.IdTraining.Length > 0)
            {
                string[] cursuriIdArray = concluziiEvaluare.IdTraining.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cursId in cursuriIdArray)
                {
                    int idCurs = 0;
                    int.TryParse(cursId, out idCurs);
                    if (idCurs != 0) 
                    {
                        var curs = _efCursuriRepository.Get(idCurs);
                        if (curs!= null)
                        {
                            listaTraininguri.Add(curs);
                        }
                    }
                }
            }

            return new PdfEvaluareConcluziiModel{
                Anul = anul.HasValue ? anul.Value : DateTime.Now.Year,
                DateEvaluare = dateEvaluare,
                ConclzuieEvaluare = concluziiEvaluare, 
                Header = headerData,
                ListaTrainiguri = listaTraininguri.ToArray()
            };
        }
    }
}