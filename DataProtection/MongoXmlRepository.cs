using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using learnCore;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;

namespace LabirunServer.DataProtection
{
    public sealed class MongoXmlRepository : IXmlRepository
    {
        private readonly IMongoCollection<MongoStoredKey> Collection;
        private readonly FilterDefinitionBuilder<MongoStoredKey> Filters = Builders<MongoStoredKey>.Filter;

        public MongoXmlRepository(IMongoDatabase db, string collectionName)
        {
            Collection = db.GetCollection<MongoStoredKey>(collectionName);
        }

        public MongoXmlRepository(MongoDBContext context)
        {
            Collection = context.GetProtectionCollection();
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var keyElements = Collection.Find(Filters.Exists(x => x.Xml)).ToList();
            return keyElements.Select(ConvertToXml).ToList().AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var keyElement = new MongoStoredKey
            {
                Xml = element.ToString(SaveOptions.DisableFormatting),
                FriendlyName = friendlyName
            };

            keyElement.Validate();
            Collection.InsertOne(keyElement);
        }


        private static XElement ConvertToXml(MongoStoredKey element)
        {
            element.Validate();
            var result = XElement.Parse(element.Xml);

            return result;
        }
    }
}