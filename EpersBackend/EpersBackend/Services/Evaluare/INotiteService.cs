using Epers.Models.Evaluare;

namespace EpersBackend.Services.Evaluare
{
    public interface INotiteService
    {
        List<Notite> GetNotiteUser(int idAngajat, int? anul = null);

        void Add(Notite nota);
    }
}

