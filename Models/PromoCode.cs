using MongoDB.Bson.Serialization.Attributes;

namespace LabirunModel.Labirun
{
    public class PromoCode
    {
        [BsonId]
        public string Id { get; set; }
        public string Code { get; set; }
    }
}