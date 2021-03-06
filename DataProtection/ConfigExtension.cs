using DieupeGames.Data.Mongo;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DieupeGames.DataProtection
{
    public class ConfigExtension2
    {
       
    }
    public static class ConfigExtension
    {
        public static IDataProtectionBuilder PersistKeysToMongoDb(this IDataProtectionBuilder builder,
            MongoDBContext context)
        {
            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new MongoXmlRepository(context);
            });

            return builder;
        }

        public static IDataProtectionBuilder PersistKeysToMongoDb(this IDataProtectionBuilder builder,
          IMongoDatabase db, string collectionName)
        {

            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new MongoXmlRepository(db, collectionName);
            });

            return builder;
        }
    }
}