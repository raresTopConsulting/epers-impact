using System.Text.Json.Serialization;

namespace Epers.Models.Obiectiv
{
    public class SalesForceImpactModel
    {
        [JsonPropertyName("agentId")]
        public string AgentId { get; set; } = default!;

        /// <summary>Agent’s full name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        /// <summary>Email address.</summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;

        /// <summary>Start of the reporting period – only a date (no time zone).</summary>
        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        /// <summary>End of the reporting period – only a date (no time zone).</summary>
        [JsonPropertyName("end")]
        public DateTime End { get; set; }

        /// <summary>All the numeric metrics for the period.</summary>
        [JsonPropertyName("metrics")]
        public Metrics Metrics { get; set; } = default!;

        /// <summary>When the record was last synced – ISO‑8601 UTC timestamp.</summary>
        [JsonPropertyName("syncedAt")]
        public DateTimeOffset SyncedAt { get; set; }
    }

    public class Metrics
    {
         [JsonPropertyName("leaduriTotal")]
        public int LeaduriTotal { get; set; }

        [JsonPropertyName("leaduriRamase")]
        public int LeaduriRamase { get; set; }

        [JsonPropertyName("telefoane")]
        public int Telefoane { get; set; }

        [JsonPropertyName("mesaje")]
        public int Mesaje { get; set; }

        // The following two property names contain non‑ASCII characters.
        // We keep the C#‑friendly names (`Intântâlniri`, `Intrevizionări`) and map
        // them to the JSON names with JsonPropertyName.
        [JsonPropertyName("întâlniri")]
        public int Intânlniri { get; set; }   // <-- property name is *Intânlniri* (ASCII‑only)

        [JsonPropertyName("revizionări")]
        public int Revizionări { get; set; }   // <-- property name is *Revizionări* (ASCII‑only)

        [JsonPropertyName("semnăriNoi")]
        public int SemnăriNoi { get; set; }   // <-- property name is *SemnăriNoi*

        [JsonPropertyName("valoareSemnăriNoi")]
        public int ValoareSemnăriNoi { get; set; }   // <-- property name is *ValoareSemnăriNoi*

        [JsonPropertyName("cvcCount")]
        public int CvcCount { get; set; }

        [JsonPropertyName("cvcValue")]
        public int CvcValue { get; set; }
    }
}