using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Shared.MongoDB
{
    public class DBClient
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        // Connection string
        private static string connectionUri { get; set; }

        public DBClient(string db, string connectionString)
        {
            connectionUri = connectionString;
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase(db);
        }

        private  IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public void RegisterClassMap<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                BsonClassMap.RegisterClassMap<T>();
        }

        public async Task<List<T>> GetAll<T>()
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Empty;

            return await Collection.Find(fil).ToListAsync();
        }

        public async Task<List<T>> GetAll<T>(string prop, long value)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Eq(prop, value);

            return await Collection.Find(fil).ToListAsync();
        }

        public async Task<List<T>> GetAll<T>(string collection)
        {
            var Collection = _database.GetCollection<T>(collection);

            var fil = Builders<T>.Filter.Empty;

            return await Collection.Find(fil).ToListAsync();
        }

        public async Task<List<T>> GetAll<T>(string prop, string value)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Regex(prop, $"/{value}/i");

            return await Collection.Find(fil).ToListAsync();
        }

        public async Task<T> Get<T>(string prop, string value)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Regex(prop, $"/{value}/i");

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<T> Get<T>(string prop, long value)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Eq(prop, value);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<T> Get<T>(string collectionName, string prop, string value)
        {
            var Collection = GetCollection<T>(collectionName);

            var fil = Builders<T>.Filter.Regex(prop, $"/{value}/i");

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<T> GetById<T>(string id)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Eq("Id", id);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<T> GetById<T>(long id)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Eq("Id", id);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<T> GetByObjectId<T>(string id)
        {
            var Collection = GetCollection<T>();

            var ObjectId = new ObjectId(id);
            var fil = Builders<T>.Filter.Eq("Id", ObjectId);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }
        public async Task<T> GetByObjectId<T>(ObjectId id)
        {
            var Collection = GetCollection<T>();

            var fil = Builders<T>.Filter.Eq("Id", id);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }
        public async Task<T> GetByObjectId<T>(string collection, string id)
        {
            var Collection = GetCollection<T>(collection);

            var ObjectId = new ObjectId(id);
            var fil = Builders<T>.Filter.Eq("Id", ObjectId);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }
        public async Task<T> GetByObjectId<T>(string collection, ObjectId id)
        {
            var Collection = GetCollection<T>(collection);

            var fil = Builders<T>.Filter.Eq("Id", id);

            var res = await Collection.Find(fil).ToListAsync();

            return res.FirstOrDefault();
        }

        public async Task<bool> UpsertOneAsync<T>(T updateRecord, string existingId)
        {
            var Collection = GetCollection<T>();

            var existingRecord = await GetById<T>(existingId);

            if (existingRecord != null)
            {
                var fil = Builders<T>.Filter.Eq("Id", existingId);
                var res = await Collection.ReplaceOneAsync(fil, updateRecord);

                return res.IsAcknowledged;
            }
            else
            {
                await Collection.InsertOneAsync(updateRecord);

                return true;
            }
        }
        public async Task<bool> UpsertOneAsync<T>(string collectionName, T updateRecord, ObjectId existingId)
        {
            var Collection = GetCollection<T>(collectionName);

            var existingRecord = await GetByObjectId<T>(collectionName, existingId);

            if (existingRecord != null)
            {
                var fil = Builders<T>.Filter.Eq("Id", existingId);
                var res = await Collection.ReplaceOneAsync(fil, updateRecord);

                return res.IsAcknowledged;
            }
            else
            {
                await Collection.InsertOneAsync(updateRecord);

                return true;
            }
        }

        public async Task<bool> UpsertOneAsync<T>(T updateRecord, ObjectId existingId)
        {
            var Collection = GetCollection<T>();

            var existingRecord = await GetByObjectId<T>(existingId);

            if (existingRecord != null)
            {
                var fil = Builders<T>.Filter.Eq("Id", existingId);
                var res = await Collection.ReplaceOneAsync(fil, updateRecord);

                return res.IsAcknowledged;
            }
            else
            {
                await Collection.InsertOneAsync(updateRecord);

                return true;
            }
        }

        public async Task<bool> InsertOneAsync<T>(T newRecord)
        {
            var Collection = GetCollection<T>();

            await Collection.InsertOneAsync(newRecord);

            return true;
        }
        public async Task<bool> InsertOneAsync<T>(string collectionName, T newRecord)
        {
            var Collection = GetCollection<T>(collectionName);

            await Collection.InsertOneAsync(newRecord);

            return true;
        }
    }
}
