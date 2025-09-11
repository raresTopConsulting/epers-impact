using Epers.DataAccess;
using Epers.Models.Nomenclatoare;

namespace EpersBackend.Services.Nomenclatoare
{
	public class EfStariPIPRepository: IEfStariPIPRepository
	{
        private readonly EpersContext _epersContext;
        private readonly ILogger<NStariPIP> _logger;

        public EfStariPIPRepository(EpersContext epersContext,
            ILogger<NStariPIP> logger)
		{
            _epersContext = epersContext;
            _logger = logger;
        }

        public NStariPIP Get(int idStare)
        {
            try
            {
                return _epersContext.NStariPIP.Single(lc => lc.Id == idStare);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stare PIP: Get - Stare PIP");
                throw;
            }
        }

    }
}

