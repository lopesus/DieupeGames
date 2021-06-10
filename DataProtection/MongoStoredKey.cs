using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DieupeGames.DataProtection
{
    public sealed class MongoStoredKey
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string Xml { get; set; }

        public string FriendlyName { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace((Xml)))
            {
                throw new InvalidOperationException();
            }
        }
    }
}