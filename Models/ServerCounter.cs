using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace LabirunModel.Labirun
{
    public partial class ServerCounter
    {
        [BsonId]
        public string Id { get; set; }
        public long Counter { get; set; }

    }
}