using Epers.Models.Afisare;

namespace EpersBackend.Services.Header
{
	public interface IHeaderService
	{
        AfisareHeaderModel GetHeader(int idAngajat);
        AfisareUserDetails GetUserDetails(int id);
    }
}

