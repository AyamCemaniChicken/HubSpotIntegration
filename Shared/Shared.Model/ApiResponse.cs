using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
    public class ApiResponse
    {
        public Dictionary<string, string>? ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
    }
}