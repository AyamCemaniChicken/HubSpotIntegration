using Shared.Helper;

namespace HubSpotIntegration.Definitions
{
    public class HubSpotApiHelper : ApiHelper
    {
        public HubSpotApiHelper(string apiKey)
        {
            RequestHeaders = new Dictionary<string, string>();
            RequestHeaders.Add("hapikey", apiKey);
        }
    }
}

