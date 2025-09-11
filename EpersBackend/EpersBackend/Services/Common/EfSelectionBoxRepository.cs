using Epers.DataAccess;
using Epers.Models;

namespace EpersBackend.Services.Common
{
    public class EfSelectionBoxRepository : IEfSelectionBoxRepository
    {
        private readonly EpersContext _epersContext;

        public EfSelectionBoxRepository(EpersContext epersContext)
        {
            _epersContext = epersContext;
        }

        public SelectionBoxes GetSelections()
        {
            return new SelectionBoxes {
                JudeteSelection = GetJudete(),
                FrecventaObiectiveSelection = GetFrecventeObiective(),
                TipObiectiveSelection = GetTipuriObiective(),
                TipCompetenteSelection = GetTipuriCompetente()
            };
        }

        public List<SelectionBox> GetFrecventeObiective()
        {
            return _epersContext.SelectionBox.Where(sb =>
                sb.Sectiune.Contains("Frecventa Obiectiv")).ToList();
        }

        public List<SelectionBox> GetJudete()
        {
            return _epersContext.SelectionBox.Where(sb =>
                 sb.Sectiune.Equals("Judet")).ToList();

        }

        public List<SelectionBox> GetTipuriCompetente()
        {
            return _epersContext.SelectionBox.Where(sb =>
                 sb.Sectiune.Equals("TipSkill")).ToList();
        }

        public List<SelectionBox> GetTipuriObiective()
        {
            var tipuriOb = from selbox in _epersContext.SelectionBox
                           where selbox.Sectiune.Equals("TipObiectiv")

                           select new SelectionBox
                           {
                               Id = selbox.Id,
                               Sectiune = selbox.Sectiune,
                               Valoare = selbox.Valoare
                           };

            return tipuriOb.ToList();
        }
    }
}

