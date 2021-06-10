using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DieupeGames.Helpers;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;
using MongoDB.Driver;

namespace DieupeGames.Data.Mongo
{
    public class PlayerDataRepository : IGenericRepository<PlayerData>
    {
        //private readonly IGameContext _context;
        private readonly IMongoCollection<PlayerData> context;

        public PlayerDataRepository(IMongoCollection<PlayerData> context)
        {
            this.context = context;
        }

        public async Task SaveList(List<PlayerData> list)
        {
            await context.InsertManyAsync(list);
        }

        public async Task<BulkWriteResult<PlayerData>> BulkUpdate(List<WriteModel<PlayerData>> writeModels)
        {
            // WriteModel<PlayerData> writeModel=new UpdateOneModel<PlayerData>();
            //List<WriteModel<PlayerData>> writeModels = new List<WriteModel<PlayerData>>();
            var result = await context.BulkWriteAsync(writeModels);
            return result;
        }

        public async Task<PlayerData> UpdateInventoryItem(string playerId, ItemId itemId, int count)
        {
            //var filter = Builders<PlayerData>.Filter.Where(
            //    p => p.Id == playerId
            //         && p.Inventory.AllItems.Any(i => i.ItemId == itemId));

            //var update = Builders<PlayerData>.Update.Set(x => x.Inventory.AllItems[-1].Count, count);


            var filter = MongoDbTools.CreateInventoryFilter(playerId, itemId);
            var update = MongoDbTools.CreateInventoryUpdate( itemId, count);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PlayerData>
            {
                ReturnDocument = ReturnDocument.After,
            };
            var playerData = await context.FindOneAndUpdateAsync(
                filter,
                update,
                findOneAndUpdateOptions);

            return playerData;
        }

        public async Task<PlayerData> FindAndUpdate(FilterDefinition<PlayerData> filterDefinition, UpdateDefinition<PlayerData> updateDefinition)
        {
            //context.FindOneAndUpdate(p => p.Id == player.Id, player);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PlayerData>
            {
                ReturnDocument = ReturnDocument.After,
            };
            var playerData = await context.FindOneAndUpdateAsync(
                filterDefinition,
                updateDefinition,
                findOneAndUpdateOptions);

            return playerData;
        }
        public async Task<PlayerData> FindAndUpdate(PlayerData player, UpdateDefinition<PlayerData> updateDefinition)
        {
            var filterDefinition = Builders<PlayerData>.Filter.Eq(p => p.Id, player.Id);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PlayerData>
            {
                ReturnDocument = ReturnDocument.After,
            };
            var playerData = await context.FindOneAndUpdateAsync(
                filterDefinition,
                updateDefinition,
                findOneAndUpdateOptions);

            return playerData;
        }

        public async Task<UpdateResult> Increment(FilterDefinition<PlayerData> filter,
            UpdateDefinition<PlayerData> update)
        {
            UpdateResult res = await context.UpdateOneAsync(filter, update);
            return res;
        }

        //public async Task<UpdateResult> Increment2(Expression<Func<PlayerData, bool>> exp,
        //    UpdateDefinition<PlayerData> update)
        //{
        //    UpdateResult res = await context.UpdateOneAsync(new ExpressionFilterDefinition<PlayerData>(exp), update);
        //    return res;
        //}


        public async Task<IEnumerable<PlayerData>> GetAll()
        {
            return await context
                .Find(_ => true)
                .ToListAsync();
        }

        //public Task<PlayerData> Find(string name)
        //{
        //    FilterDefinition<PlayerData> filter = Builders<PlayerData>.Filter.Eq(m => m.Id, name);

        //    return context
        //        .Find(filter)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<PlayerData> Find(Expression<Func<PlayerData, bool>> exp)
        {
            //FilterDefinition<PlayerData> filter = Builders<PlayerData>.Filter.Eq(m => m.Id, name);
            //context.FindAsync(p => p.Id == "",);
            return await context
                .Find(exp)
                .FirstOrDefaultAsync();
        }


        public async Task<long> Count(Expression<Func<PlayerData, bool>> exp)
        {
            return await context.CountDocumentsAsync(exp);
        }

        public async Task Create(PlayerData entity)
        {
          await context.InsertOneAsync(entity);
        }




        public async Task<PlayerData> Update0(PlayerData player)
        {
            //context.FindOneAndUpdate(p => p.Id == player.Id, player);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PlayerData>
            {
                ReturnDocument = ReturnDocument.After,
                // IsUpsert = true
            };
            FilterDefinition<PlayerData> filterDefinition = new ExpressionFilterDefinition<PlayerData>(p => p.Id == player.Id);
            UpdateDefinition<PlayerData> updateDefinition = new ObjectUpdateDefinition<PlayerData>(player);
            var playerData = await context.FindOneAndUpdateAsync(
                filterDefinition,
                updateDefinition,
                findOneAndUpdateOptions);

            if (playerData != null)
            {
                // The document already existed and was updated.
            }
            else
            {
                // The document did not exist and was inserted.
            }

            return playerData;


            //ReplaceOneResult updateResult =
            //    await context
            //        .ReplaceOneAsync(
            //            filter: g => g.Id == player.Id,
            //            replacement: player);

            //return updateResult.IsAcknowledged
            //       && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string name)
        {
            FilterDefinition<PlayerData> filter = Builders<PlayerData>.Filter.Eq(m => m.Id, name);

            DeleteResult deleteResult = await context
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}