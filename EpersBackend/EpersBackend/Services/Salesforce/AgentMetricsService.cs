

using System.Text.Json;
using Epers.Models.Salesforce;

namespace EpersBackend.Services.Salesforce
{
    public class AgentMetricsService : IAgentMetricsService
    {
        private readonly IEfAgentMetricsRepository _efAgentMetricsRepository;
        private readonly ILogger<AgentMetricsService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AgentMetricsService(IEfAgentMetricsRepository efAgentMetricsRepository,
            ILogger<AgentMetricsService> logger,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _efAgentMetricsRepository = efAgentMetricsRepository;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<int> SyncAgentMetricsFromSalesforce()
        {
            var apiUrl = _configuration["Impact:SalesForceApiUrl"];
            var apiToken = _configuration["Impact:SalesForceApiToken"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);

            var response = await _httpClient.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch Salesforce metrics: {StatusCode}", response.StatusCode);
                return 0;
            }
            var content = response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<AgentMetricsApiResponse>(content.Result);
            Console.WriteLine(apiResponse);

            if (apiResponse == null)
                return 0;
            
            int insertCount = 0;
            foreach (var agent in apiResponse.Agents)
            {
                var agentMetrics = new AgentMetrics
                {
                    IdAgent = agent.IdAgent,
                    Name = agent.Name,
                    Email = agent.Email,
                    StartDate = agent.StartDate,
                    EndDate = agent.EndDate,
                    SyncedAt = agent.SyncedAt,
                    LeaduriTotal = agent.Metrics.LeaduriTotal,
                    LeaduriRamase = agent.Metrics.LeaduriRamase,
                    Telefoane = agent.Metrics.Telefoane,
                    Mesaje = agent.Metrics.Mesaje,
                    Intalniri = agent.Metrics.Intalniri,
                    SemnariNoi = agent.Metrics.SemnariNoi,
                    ValoareSemnariNoi = agent.Metrics.ValoareSemnariNoi,
                    CvcCount = agent.Metrics.CvcCount,
                    CvcValue = agent.Metrics.CvcValue
                };

                _efAgentMetricsRepository.Upsert(agentMetrics);

                insertCount++;
            }
            return insertCount;
        }
    }
}