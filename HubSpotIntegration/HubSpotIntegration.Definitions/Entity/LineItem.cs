using System;
using System.Text.Json.Serialization;
using Shared.Common.Attributes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HubSpotIntegration.Definitions.Entity
{
    [Endpoint("crm/v3/objects/line_items")]
    public class LineItem: BaseProperties
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("hs_lastmodifieddate")]
        public DateTime? LastModifiedDate { get; set; }
        [JsonPropertyName("hs_product_id")]
        public string ProductId { get; set; }
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
    }
}