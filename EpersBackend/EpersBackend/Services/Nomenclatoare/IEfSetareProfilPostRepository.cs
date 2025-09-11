using Epers.Models.Afisare;
using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public interface IEfSetareProfilPostRepository
	{
		List<TableSetareProfil> Get(int idPost);
		void Update(SetareProfil setareProfil);
        //void Upsert(SetareProfil[] setareProfilArray);
        void Add(SetareProfil setareProfil);
		void Delete(int idProfilPost);
    }
}

