using Epers.Models;

namespace EpersBackend.Services.Common
{
	public interface IEfSelectionBoxRepository
	{
		SelectionBoxes GetSelections();
		List<SelectionBox> GetJudete();
		List<SelectionBox> GetFrecventeObiective();
		List<SelectionBox> GetTipuriObiective();
        List<SelectionBox> GetTipuriCompetente();
    }
}

