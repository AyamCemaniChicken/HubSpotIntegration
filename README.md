# HubSpotIntegration
A CLI Application and libraries for syncing HubSpot CRM data to Mongo Atlas

Arguments:
    -k --apiKey: HubSpot API Key | Required
    -d --database: The name of the DB instance you want to access. If it does not exist, it will be created. | Requires
    -e --endpoint: Single endoiint you would like to run represented by a number (see possible values below)
    -o --endpoints: A list of all endpoints you wouod like to run represented by a series of numbers sepetated by spaces. (Exclude -e and -o to default to all endpoints)
    -f --offset: The number of days prior to the current day you want to filter by. (Exclude to default to a full sync)

Possible Endpoint Values:
    1 Contacts
    2 Products
    3 Deals
    4 Line Items
    5 Associations

**NOTE**
Currently, the connection string for MongoDB client is hardcoded. Replace the placeholder string in Shared/Shared.MongoDB/DBClient.cs - Line 13 with your atlas connection string.
    
