using System;
using System.Text.Json.Serialization;
using HubSpotIntegration.Definitions.Entity;

namespace HubSpotIntegration.Definitions.Response
{
    public class Response<X> where X: BaseProperties
    {
        [JsonPropertyName("results")]
        public List<BaseEntity<X>> Results { get; set; }
        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }
    }
}