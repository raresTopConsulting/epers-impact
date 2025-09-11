using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public interface IEfStariPIPRepository
    {
        NStariPIP Get(int idStare);
    }
}

