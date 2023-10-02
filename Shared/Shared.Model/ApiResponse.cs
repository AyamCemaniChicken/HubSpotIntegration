
using System.Net;


namespace Shared.Helper
{
    public class ApiResponse
    {
        public Dictionary<string, string>? ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}