using System;
using System.Text.Json.Serialization;
using Shared.Common.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HubSpotIntegration.Definitions.Entity
{
    [Endpoint("crm/v3/objects/deals")]
    public class Deal: BaseProperties
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("closedate")]
        public string CloseDate { get; set; }
        [JsonPropertyName("dealname")]
        public string Name { get; set; }
        [JsonPropertyName("dealstage")]
        public string Stage { get; set; }
        [JsonPropertyName("hubspot_owner_id")]
        public string OwnerId { get; set; }
        [JsonPropertyName("pipeline")]
        public string Pipeline { get; set; }
        [JsonPropertyName("hs_lastmodifieddate")]
        public DateTime? LastModifiedDate { get; set; }
    }
}