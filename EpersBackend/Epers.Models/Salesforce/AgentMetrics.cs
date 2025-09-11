using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Epers.Models.Salesforce
{   
    /// <summary>
    /// Represents a single row in the AgentMetrics table.
    /// </summary>
    [Table("AgentMetrics", Schema = "dbo")]
    public class AgentMetrics
    {
        /// <summary>
        /// Primary key – the _id string coming from your JSON.
        /// </summary>
        [Key]
        [JsonPropertyName("_id")]
        [Column("Id")]
        public string Id { get; set; }

        [JsonPropertyName("start")]
        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("name")]
        [Column("Name")]
        public string Name { get; set; }

        [JsonPropertyName("end")]
        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("agentId")]
        [Column("IdAgent")]
        public string IdAgent { get; set; }

        [JsonPropertyName("email")]
        [Column("Email")]
        public string Email { get; set; }

        [JsonPropertyName("leaduriTotal")]
        [Column("Leaduri_total")]
        public int LeaduriTotal { get; set; }

        [JsonPropertyName("leaduriRamase")]
        [Column("Leaduri_ramase")]
        public int LeaduriRamase { get; set; }

        [JsonPropertyName("telefoane")]
        [Column("Telefoane")]
        public int Telefoane { get; set; }

        [JsonPropertyName("mesaje")]
        [Column("Mesaje")]
        public int Mesaje { get; set; }

        [JsonPropertyName("intalniri")]
        [Column("Intalniri")]
        public int Intalniri { get; set; }

        [JsonPropertyName("syncedAt")]
        [Column("SyncedAt")]
        public DateTime SyncedAt { get; set; }   // stored as UTC in the DB
    }
}