using CommandLine;
namespace Shared.Model
{
    public class CLIParams<T> where T: Enum
    {
        [Option('d', "database", Required = false, HelpText = "Database Name")]
        public string Db { get; set; }

        [Option('e', "endpoint", Required = false, HelpText = "Specify a single endpoint to run")]
        public T? Endpoint { get; set; }

        [Option('o', "endpoints", Required = false, HelpText = "Specify an array of Endpoints to run")]
        public IEnumerable<T>? Endpoints { get; set; }

        [Option('f', "offset", Required = false, HelpText = "Specify how many days before today to run")]
        public int? DaysPrior { get; set; }

        [Option('k', "apiKey", Required = true, HelpText = "Api key required to access the CRM API")]
        public string ApiKey { get; set; }
    }
}

