using System.Text.Json.Serialization;

namespace Epers.Models.Salesforce
{
    public class AgentMetricsApiResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("agents")]
        public List<AgentMetricsDto> Agents { get; set; }
    }

    public class AgentMetricsDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("start")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("agentId")]
        public string IdAgent { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("metrics")]
        public MetricsDto Metrics { get; set; }

        [JsonPropertyName("syncedAt")]
        public DateTime SyncedAt { get; set; }
    }

    public class MetricsDto
    {
        [JsonPropertyName("leaduriTotal")]
        public int LeaduriTotal { get; set; }

        [JsonPropertyName("leaduriRamase")]
        public int LeaduriRamase { get; set; }

        [JsonPropertyName("telefoane")]
        public int Telefoane { get; set; }

        [JsonPropertyName("mesaje")]
        public int Mesaje { get; set; }

        [JsonPropertyName("intalniri")]
        public int Intalniri { get; set; }

        [JsonPropertyName("semnariNoi")]
        public int SemnariNoi { get; set; }

        [JsonPropertyName("valoareSemnariNoi")]
        public decimal ValoareSemnariNoi { get; set; }

        [JsonPropertyName("cvcCount")]
        public int CvcCount { get; set; }

        [JsonPropertyName("cvcValue")]
        public decimal CvcValue { get; set; }
    }

}