
using System.Text.Json.Serialization;
using Shared.Common.Attributes;

namespace HubSpotIntegration.Definitions.Entity
{
    [Endpoint("crm/v3/objects/products")]
    public class Product: BaseProperties
    {
        [JsonPropertyName("hs_cost_of_goods_sold")]
        public string CostOfGoodsSold { get; set; }
        [JsonPropertyName("hs_recurring_billing_period")]
        public string RecurringBillingPeriod { get; set; }
        [JsonPropertyName("hs_sku")]
        public string SKU { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public string Price { get; set; }
        [JsonPropertyName("hs_lastmodifieddate")]
        public DateTime? LastModifiedDate { get; set; }
    }
}