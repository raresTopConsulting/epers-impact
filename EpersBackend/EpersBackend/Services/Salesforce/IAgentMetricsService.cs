

namespace EpersBackend.Services.Salesforce
{
    public interface IAgentMetricsService
    {
        Task<int> SyncAgentMetricsFromSalesforce();
    }
}