using Epers.DataAccess;
using Epers.Models.Salesforce;

namespace EpersBackend.Services.Salesforce
{
    public class EfAgentMetricsRepository : IEfAgentMetricsRepository
    {
        private readonly EpersContext _epersContext;
        private readonly ILogger<AgentMetrics> _logger;

        public EfAgentMetricsRepository(EpersContext epersContext,
            ILogger<AgentMetrics> logger)
        {
            _epersContext = epersContext;
            _logger = logger;
        }

        public void Add(AgentMetrics agentData)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.AgentMetrics.Add(agentData);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfAgentMetricsRepository: Add");
                throw;
            }
        }

        public AgentMetrics Get(string id)
        {
            try
            {
                return _epersContext.AgentMetrics.Single(am => am.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: Get");
                throw;
            }
        }

        public List<AgentMetrics> GetAll()
        {
            try
            {
                return _epersContext.AgentMetrics.Select(am => am).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: GetAll");
                throw;
            }
        }

        public AgentMetrics GetByAgentId(string idAgent)
        {
            try
            {
                return _epersContext.AgentMetrics.Single(am => am.IdAgent == idAgent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfPIPRepository: Get");
                throw;
            }
        }

        public void Update(AgentMetrics agentData)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    _epersContext.AgentMetrics.Update(agentData);
                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfAgentMetricsRepository: Update");
                throw;
            }
        }

        public void Upsert(AgentMetrics agentData)
        {
            try
            {
                using (var dbTransaction = _epersContext.Database.BeginTransaction())
                {
                    var existing = _epersContext.AgentMetrics
                        .FirstOrDefault(am => am.IdAgent == agentData.IdAgent);

                    if (existing != null)
                    {
                        existing.StartDate = agentData.StartDate;
                        existing.EndDate = agentData.EndDate;
                        existing.SyncedAt = agentData.SyncedAt;
                        existing.LeaduriTotal = agentData.LeaduriTotal;
                        existing.LeaduriRamase = agentData.LeaduriRamase;
                        existing.Telefoane = agentData.Telefoane;
                        existing.Mesaje = agentData.Mesaje;
                        existing.Intalniri = agentData.Intalniri;
                        existing.SemnariNoi = agentData.SemnariNoi;
                        existing.ValoareSemnariNoi = agentData.ValoareSemnariNoi;
                        existing.CvcCount = agentData.CvcCount;
                        existing.CvcValue = agentData.CvcValue;

                        _epersContext.AgentMetrics.Update(existing);
                    }
                    else
                    {
                        _epersContext.AgentMetrics.Add(agentData);
                    }

                    _epersContext.SaveChanges();
                    dbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EfAgentMetricsRepository: Upsert");
                throw;
            }
        }
    }
}