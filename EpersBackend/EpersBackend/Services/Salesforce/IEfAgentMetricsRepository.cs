using Epers.Models.Salesforce;

namespace EpersBackend.Services.Salesforce
{
    public interface IEfAgentMetricsRepository
    {
        AgentMetrics Get(string id);
        AgentMetrics GetByAgentId(string id);
        List<AgentMetrics> GetAll();
        void Update(AgentMetrics agentData);
        void Add(AgentMetrics agentData);
        void Upsert(AgentMetrics agentData);
    }
}