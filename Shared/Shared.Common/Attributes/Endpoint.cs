
namespace Shared.Common.Attributes
{
    public class EndpointAttribute: Attribute
    {
        public string Url { get; set; }

        public EndpointAttribute(string url)
        {
            Url = url;
        }
    }
}