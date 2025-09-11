using Epers.Models.Nomenclatoare;
using Epers.Models.PIP;
using Epers.Models.Users;

namespace EpersBackend.Services.PIP
{
	public interface IPIPService
	{
		PipDisplayAddEditModel CreateInitial(int idAngajat);
		PipDisplayAddEditModel Get(int idAngajat, int? anul = null);
		void Add(PipDisplayAddEditModel pipDisplayAddEditModel);
		void Update(PipDisplayAddEditModel pipDisplayAddEditModel);
		bool HasPip(int idAngajat, int? anul = null);
		bool HasPipNefinalizat(int idAngajat, int? anul = null);
		SubalterniDropdown[] SubalterniThatHavePIP(string? matricolaSuperior = null);
		string ActualizareListaSublaterniThatNeedPip(int? anul = null);
		ListaSubalterniPipDisplayModel GetListaSubalterniOngoingPipForAdmin(int currentPage,
			int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null);
		ListaSubalterniPipDisplayModel GetListaSubalterniOngoingPip(string matricolaSuperior,
			int currentPage, int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null);
		ListaSubalterniPipDisplayModel GetListaSubalterniIstoricPipForAdmin(int currentPage,
			int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null);
		ListaSubalterniPipDisplayModel GetListaSubalterniIstoricPip(string matricolaSuperior,
			int currentPage, int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null);
		ListaSubalterniCalificatiPipDisplayModel GetListaSublaterniCalaificatiPipForAdmin(int currentPage,
			int itemsPerPage, int? idFirmaFilter = null, int? anul = null, string? filter = null);
		ListaSubalterniCalificatiPipDisplayModel GetListaSubalterniPentruAprobarePip(int currentPage,
			int itemsPerPage, int? idFirmaFilter = null, int ? anul = null, string? filter = null);
		bool AngajatAreMediaPtPip(int idAngajat, int? anul = null);
		NStariPIP GetStareActualaPip(int idAngajat, int? anul = null);
	}
}

