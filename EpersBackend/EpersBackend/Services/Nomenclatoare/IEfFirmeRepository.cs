using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public interface IEfFirmeRepository
    {
        NFirme Get(int id);
        FirmeDisplayModel GetAllPaginated(int currentPage, int itemsPerPage,
            string? filter = null, int? firmaFilter = null);
        NFirme[] GetAll();
        void Update(NFirme firma);
		void Add(NFirme firma);
		void SetToInactive(int id);
	}
}

