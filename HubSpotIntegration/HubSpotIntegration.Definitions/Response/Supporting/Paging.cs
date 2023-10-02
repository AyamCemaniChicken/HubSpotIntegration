
using System.Text.Json.Serialization;

namespace HubSpotIntegration.Definitions.Response
{
    public class Paging 
    {
        [JsonPropertyName("next")]
        public Next Next { get; set; }
    }

    public class Next 
    {
        [JsonPropertyName("after")]
        public string After { get; set; }
        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}