using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabirunModel.Labirun;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace DieupeGames
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            BsonClassMap.RegisterClassMap<ServerCounter>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<PromoCode>(cm =>
                       {
                           cm.AutoMap();
                           cm.MapIdMember(c => c.Id)
                               .SetIdGenerator(new StringObjectIdGenerator())
                               .SetSerializer(new StringSerializer(BsonType.ObjectId));

                           cm.SetIgnoreExtraElements(true);
                       });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
