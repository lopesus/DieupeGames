using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LabirunModel.Labirun;
using MongoDB.Driver;

namespace learnCore
{
    public class ServerCounterRepository
    {
        //private readonly IGameContext _context;
        private readonly IMongoCollection<ServerCounter> context;

        public ServerCounterRepository(IMongoCollection<ServerCounter> context)
        {
            this.context = context;
        }



        public async Task<long> Count()
        {
            return await context.CountDocumentsAsync(FilterDefinition<ServerCounter>.Empty);
        }

        public async Task<ServerCounter> GetCounter()
        {
            return await context
                    .Find(FilterDefinition<ServerCounter>.Empty)
                    .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<ServerCounter>> GetAll()
        {
            return await context
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task Initialise()
        {
            await context.InsertOneAsync(new ServerCounter() { Counter = 20 });
        }

        public async Task<ServerCounter> GetNextCounter()
        {
            try
            {
                //var id = MongoDB.Bson.ObjectId.GenerateNewId(new DateTime(1981)).ToString();

                var filterDefinition = Builders<ServerCounter>.Filter.Empty;
                //var filterDefinition = Builders<ServerCounter>.Filter.Eq(m => m.Id, id);

                var findOneAndUpdateOptions = new FindOneAndUpdateOptions<ServerCounter>
                {
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = true,
                };

                var update = Builders<ServerCounter>.Update.Inc(c => c.Counter, 1);
                var serverCounter = await context.FindOneAndUpdateAsync(
                    filterDefinition,
                    update,
                    findOneAndUpdateOptions);

                return serverCounter;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

    }
}