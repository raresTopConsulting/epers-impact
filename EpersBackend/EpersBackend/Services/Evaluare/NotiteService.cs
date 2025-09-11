using Epers.DataAccess;
using Epers.Models.Evaluare;

namespace EpersBackend.Services.Evaluare
{
	public class NotiteService: INotiteService
	{
        private readonly EpersContext _epersContext;
        private readonly ILogger<Notite> _logger;

        public NotiteService(EpersContext epersContext,
             ILogger<Notite> logger)
		{
            _epersContext = epersContext;
            _logger = logger;
        }

        public void Add(Notite mentiune)
        {
            mentiune.Data = DateTime.Now;
            mentiune.UpdateId = mentiune.IdSuperior;
            
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.Notite.Add(mentiune);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Notite: Add");
                throw;
            }
        }

        public List<Notite> GetNotiteUser(int idAngajat, int? anul = null)
        {
            if (anul.HasValue)
            {
                return _epersContext.Notite.Where(not => not.IdAngajat == idAngajat.ToString() && not.Data.Year == anul.Value).ToList();
            }
            else
            {
                return _epersContext.Notite.Where(not => not.IdAngajat == idAngajat.ToString()).ToList();
            }
        }
    }
}

