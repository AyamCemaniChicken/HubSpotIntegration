
using System.Text.Json.Serialization;

namespace HubSpotIntegration.Definitions.Entity
{
    public class BaseEntity<X> where X: BaseProperties
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("properties")]
        public X Properties { get; set; }
        [JsonPropertyName("associations")]
        public Associations? Associations { get; set; }
    }

    public class BaseProperties
    {
        [JsonPropertyName("createdate")]
        public DateTime? CreateDate { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public Associations? Associations { get; set; }
    }

    public class Associations
    {
        [JsonPropertyName("line items")]
        public AssociationResults? LineItems { get; set; }
        [JsonPropertyName("companies")]
        public AssociationResults? Companies { get; set; }
        [JsonPropertyName("contacts")]
        public AssociationResults? Contacts { get; set; }
    }

    public class AssociationResults
    {
        [JsonPropertyName("results")]
        public List<AssociationProps> Results { get; set; }
    }

    public class AssociationProps
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}