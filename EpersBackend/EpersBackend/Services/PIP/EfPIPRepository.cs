using Epers.DataAccess;
using Epers.Models.PIP;
using EpersBackend.Services.Email;
using EpersBackend.Services.Users;

namespace EpersBackend.Services.PIP
{
    public class EfPIPRepository : IEfPIPRepository
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<PlanInbunatatirePerformante> _logger;
        private readonly IEmailSendService _emailSendService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public EfPIPRepository(EpersContext epersContext,
            ILogger<PlanInbunatatirePerformante> logger,
            IEmailSendService emailSendService,
            IUserService userService,
            IConfiguration configuration)
        {
            _epersContext = epersContext;
            _logger = logger;
            _emailSendService = emailSendService;
            _userService = userService;
            _configuration = configuration;
        }

        public void Add(PlanInbunatatirePerformante pip)
        {
            try
            {
                pip.UpdateDate = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.PlanInbunatatirePerformante.Add(pip);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: Add");
                throw;
            }
        }

        public PlanInbunatatirePerformante Get(int id)
        {
            try
            {
                return _epersContext.PlanInbunatatirePerformante.Single(pip => pip.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: Get");
                throw;
            }
        }

        public void Update(PlanInbunatatirePerformante pip)
        {
            try
            {
                pip.UpdateDate = DateTime.Now;

                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.PlanInbunatatirePerformante.Update(pip);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: Update");
                throw;
            }
        }

    }
}