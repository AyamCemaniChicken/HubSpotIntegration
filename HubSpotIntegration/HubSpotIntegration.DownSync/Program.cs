using System;
using MongoDB.Driver;
using CommandLine;
using Shared.Model;
using HubSpotIntegration.DownSync.Enum;
using HubSpotIntegration.ApiSync;

namespace HubSpotIntegration.DownSync
{
    internal class Program
    {
        static CLIParams<Endpoints> Params = default(CLIParams<Endpoints>);

        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<CLIParams<Endpoints>>(args).WithParsed(RunOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void RunOptions(CLIParams<Endpoints> opt)
        {
            Params = opt;
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var DownSync = new DownSyncService();
            DownSync.init(Params.ApiKey, "https://api.hubapi.com/", Params.Db);

            var Endpoints = new List<string>();

            if (Params.Endpoints?.Count() > 0)
            {
                foreach(var endpoint in Params.Endpoints)
                {
                    Endpoints.Add(endpoint.ToString());
                }
            }
            else if (Params.Endpoint.ToString() == "")
            {
                foreach(var prop in System.Enum.GetValues(typeof(Enum.Endpoints)))
                {
                    if (prop == null)
                        continue;

                    Endpoints.Add(prop.ToString());
                }
            }
            else
                Endpoints.Add(Params.Endpoint.ToString());
            
            var Associations = new Dictionary<string, string>()
            {
                { "deals", "line_items" }
            };

            await DownSync.Run(Endpoints, Associations);
        }

    }
}