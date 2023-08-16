using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Shared.Model
{
    public class BaseFile 
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))] 
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
    }
}