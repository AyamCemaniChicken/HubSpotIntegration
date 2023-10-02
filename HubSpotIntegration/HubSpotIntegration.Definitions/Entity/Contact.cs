
using System.Text.Json.Serialization;
using Shared.Common.Attributes;

namespace HubSpotIntegration.Definitions.Entity
{
    [Endpoint("crm/v3/objects/contacts")]
    public class Contact: BaseProperties
    {
        public string Company { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("website")]
        public string Website { get; set; }
        [JsonPropertyName("lastmodifieddate")]
        public DateTime? LastModifiedDate { get; set; }
    }
}