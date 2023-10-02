
using System.Text.Json.Serialization;
using Shared.Common.Attributes;
using HubSpotIntegration.Definitions.Response;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HubSpotIntegration.Definitions.Entity
{
    [Endpoint($"crm/v4/associations")]

    public class AssociationResponse
    {
        [JsonPropertyName("results")]
        public List<Association> Results { get; set; }
        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }
    }

    public class Association
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonPropertyName("typeId")]
        public long TypeId { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }
}