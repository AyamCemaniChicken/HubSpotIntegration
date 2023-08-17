using System;
using HubSpotIntegration.Definitions;
using Shared.MongoDB;
using Shared.Model;
using HubSpotIntegration.Definitions.Entity;
using HubSpotIntegration.Definitions.Response;
using Shared.Common.Attributes;
using System.Xml.Serialization;

namespace HubSpotIntegration.ApiSync
{
    public class DownSyncService
    {
        private string AccessKey { get; set; }
        private string StartUrl { get; set; }
        private string queryParams = "?limit=20&archived=false";

        public HubSpotApiHelper apiHelper { get; set; }
        public DBClient dbClient { get; set; }
        private Config config { get; set; }

        // Initialize
        public void init(string accessKey, string startUrl, string database)
        {
            config = GetConfig();
            while (config == null)
            {
                config = CreateConfig();
            }

            var connectionString = config.ConnectionString;

            AccessKey = accessKey;
            StartUrl = startUrl;

            apiHelper = new HubSpotApiHelper(AccessKey);
            apiHelper.BaseUrl = startUrl;

            dbClient = new DBClient(database, connectionString);
        }

        public Config CreateConfig()
        {
        Start:
            Console.WriteLine("Enter your MongoDB connection string:");
            var connectionString = Console.ReadLine();
            goto WriteConfig;

        WriteConfig:
            if (connectionString == null)
                goto Start;
            var ConfigName = "config.xml";
            if (!File.Exists(ConfigName))
            {
                using (FileStream fs = new FileStream(ConfigName, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Config));
                    Config sxml = new Config()
                    {
                        ConnectionString = connectionString
                    };
                    xs.Serialize(fs, sxml);
                    return sxml;
                }
            }
            else
            {
                var config = GetConfig();
                if (config != null)
                    return config;
                else goto Start;
            }
        }

        public Config GetConfig()
        {
            var ConfigName = "config.xml";
            if (File.Exists(ConfigName))
            {
                using (FileStream fs = new FileStream(ConfigName, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Config));
                    Config sc = (Config)xs.Deserialize(fs);
                    return sc;
                }
            }

            return null;
        }

        public async Task Run(List<string> endpoints, Dictionary<string, string> associations)
        {
            apiHelper.AccessToken = new ApiToken()
            {
                Type = "Bearer",
                Token = AccessKey
            };

            if (endpoints != null && endpoints.Any(x => x == "contacts") || endpoints[0] == "contacts")
                await GetContacts();
            if (endpoints != null && endpoints.Any(x => x == "products") || endpoints[0] == "products")
                await GetProducts();
            if (endpoints != null && endpoints.Any(x => x == "deals") || endpoints[0] == "deals")
                await GetDeals(associations);
            if (endpoints != null && endpoints.Any(x => x == "lineitems") || endpoints[0] == "lineitems")
                await GetLineItems();
            if (endpoints != null && endpoints.Any(x => x == "associations") || endpoints[0] == "associations" && associations.Count > 0)
                await GetAssociations(associations);
        }
        
        public async Task GetContacts()
        {
            var after = "";
            bool running = true;

            if (!string.IsNullOrWhiteSpace(after))
                queryParams += $"&after={after}";
            
            while (running)
            {

                var endpoint = typeof(Contact).GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
                var response = await apiHelper.GetObjectResponse<Response<Contact>>($"{endpoint.Url}{queryParams}");

                if (response.Results.Count > 0)
                {
                    foreach (var item in response.Results)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Properties.ToString()))
                        {
                            var existingContact = await dbClient.Get<Contact>("Id", item.Properties.Id);

                            if (existingContact != null)
                            {
                                item.Properties.Associations = item.Associations;
                                var existingId = existingContact.Id;
                                existingContact = item.Properties;
                                existingContact.Id = existingId;

                                try
                                {
                                    await dbClient.UpsertOneAsync<Contact>(existingContact, existingId);
                                }
                                catch (Exception ex){ }
                            }
                            else
                            {
                                try
                                {
                                    item.Properties.Id = item.Id;
                                    await dbClient.InsertOneAsync<Contact>(item.Properties);
                                }
                                catch (Exception ex){ }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(response.Paging?.Next?.After))
                    {
                        after = response.Paging.Next.After;
                        continue;
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }
                else
                {
                    running = false;
                    break;
                }
            }
        }

        public async Task GetProducts()
        {
            var after = "";
            bool running = true;

            if (!string.IsNullOrWhiteSpace(after))
                queryParams += $"&after={after}";
            
            while (running)
            {

                var endpoint = typeof(Product).GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
                var response = await apiHelper.GetObjectResponse<Response<Product>>($"{endpoint.Url}{queryParams}");

                if (response.Results.Count > 0)
                {
                    foreach (var item in response.Results)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Properties.ToString()))
                        {
                            item.Properties.Associations = item.Associations;
                            var existingProduct = await dbClient.Get<Product>("Id", item.Properties.Id);

                            if (existingProduct != null)
                            {
                                var existingId = existingProduct.Id;
                                existingProduct = item.Properties;
                                existingProduct.Id = existingId;

                                try
                                {
                                    await dbClient.UpsertOneAsync<Product>(existingProduct, new MongoDB.Bson.ObjectId(existingId));
                                }
                                catch (Exception ex){ }
                            }
                            else
                            {
                                try
                                {
                                    item.Properties.Id = item.Id;
                                    await dbClient.InsertOneAsync<Product>(item.Properties);
                                }
                                catch (Exception ex){ }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(response.Paging?.Next?.After))
                    {
                        after = response.Paging.Next.After;
                        continue;
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }
                else
                {
                    running = false;
                    break;
                }
            }
        }

        public async Task GetDeals(Dictionary<string,string> associations)
        {
            var after = "";
            var associationValues = "&associations=";
            bool running = true;

            if (associations.Count() > 0)
            {
                foreach (var associationKey in associations.Keys)
                {
                    if (associationValues.Count() > 14)
                    {
                        associationValues += $",{associations[associationKey]}";
                    }
                    else
                    {
                        associationValues += $"{associations[associationKey]}";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(after))
                queryParams += $"&after={after}";
            
            while (running)
            {
                var endpoint = typeof(Deal).GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
                var response = await apiHelper.GetObjectResponse<Response<Deal>>($"{endpoint.Url}{queryParams}{associationValues}");

                if (response.Results.Count > 0)
                {
                    foreach (var item in response.Results)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Properties.ToString()))
                        {
                            item.Properties.Associations = item.Associations;
                            var existingDeal = await dbClient.Get<Deal>("Id", item.Properties.Id);

                            if (existingDeal != null)
                            {
                                var existingId = existingDeal.Id;
                                existingDeal = item.Properties;
                                existingDeal.Id = existingId;

                                try
                                {
                                    await dbClient.UpsertOneAsync<Deal>(existingDeal, new MongoDB.Bson.ObjectId(existingId));
                                }
                                catch (Exception ex){ }
                            }
                            else
                            {
                                try
                                {
                                    item.Properties.Id = item.Id;
                                    await dbClient.InsertOneAsync<Deal>(item.Properties);
                                }
                                catch (Exception ex){ }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(response.Paging?.Next?.After))
                    {
                        after = response.Paging.Next.After;
                        continue;
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }
                else
                {
                    running = false;
                    break;
                }
            }
        }

        public async Task GetLineItems()
        {
            var after = "";
            bool running = true;

            if (!string.IsNullOrWhiteSpace(after))
                queryParams += $"&after={after}";
            
            while (running)
            {

                var endpoint = typeof(LineItem).GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
                var response = await apiHelper.GetObjectResponse<Response<LineItem>>($"{endpoint.Url}{queryParams}");

                if (response.Results.Count > 0)
                {
                    foreach (var item in response.Results)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Properties.ToString()))
                        {
                            item.Properties.Associations = item.Associations;
                            var existingLineItem = await dbClient.Get<LineItem>("Id", item.Properties.Id);

                            if (existingLineItem != null)
                            {
                                var existingId = existingLineItem.Id;
                                existingLineItem = item.Properties;
                                existingLineItem.Id = existingId;

                                try
                                {
                                    await dbClient.UpsertOneAsync<LineItem>(existingLineItem, new MongoDB.Bson.ObjectId(existingId));
                                }
                                catch (Exception ex){ }
                            }
                            else
                            {
                                try
                                {
                                    item.Properties.Id = item.Id;
                                    await dbClient.InsertOneAsync<LineItem>(item.Properties);
                                }
                                catch (Exception ex){ }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(response.Paging?.Next?.After))
                    {
                        after = response.Paging.Next.After;
                        continue;
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }
                else
                {
                    running = false;
                    break;
                }
            }
        }

        public async Task GetAssociations(Dictionary<string,string> associations)
        {
            foreach (var associationKey in associations.Keys)
            {
                var objectEndpoint = $"/{associationKey}/{associations[associationKey]}/labels";
                var after = "";

                bool running = true;

                if (!string.IsNullOrWhiteSpace(after))
                    queryParams += $"&after={after}";
                
                while (running)
                {

                    var endpoint = typeof(AssociationResponse).GetCustomAttributes(typeof(EndpointAttribute), true).FirstOrDefault() as EndpointAttribute;
                    var response = await apiHelper.GetObjectResponse<AssociationResponse>($"{endpoint.Url}{objectEndpoint}{queryParams}");

                    if (response.Results.Count > 0)
                    {
                        foreach (var item in response.Results)
                        {
                            if (!string.IsNullOrWhiteSpace(item.TypeId.ToString()))
                            {
                                if (item.Label == null || item.Label == "")
                                    item.Label = $"{associationKey},{associations[associationKey]}";

                                var existingAssociation = dbClient.GetAll<Association>("TypeId", item.TypeId).Result.Where(x => x.Label == item.Label)?.FirstOrDefault();

                                if (existingAssociation != null)
                                {
                                    var existingId = existingAssociation.Id;
                                    existingAssociation = item;
                                    existingAssociation.Id = existingId;

                                    try
                                    {
                                        await dbClient.UpsertOneAsync<Association>(existingAssociation, new MongoDB.Bson.ObjectId(existingId));
                                    }
                                    catch (Exception ex){ }
                                }
                                else
                                {
                                    try
                                    { 
                                        item.TypeId = item.TypeId;
                                        await dbClient.InsertOneAsync<Association>(item);
                                    }
                                    catch (Exception ex){ }
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(response.Paging?.Next?.After))
                        {
                            after = response.Paging.Next.After;
                            continue;
                        }
                        else
                        {
                            running = false;
                            break;
                        }
                    }
                    else
                    {
                        running = false;
                        break;
                    }
                }
            }
        }
    }
}

