using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LabirunModel.Labirun;
using MongoDB.Driver;

namespace learnCore
{
    public class PromoCodeRepository : IGenericRepository<PromoCode>
    {
        //private readonly IGameContext _context;
        private readonly IMongoCollection<PromoCode> context;

        public PromoCodeRepository(IMongoCollection<PromoCode> context)
        {
            this.context = context;
        }

        public async Task SaveList(List<PromoCode> list)
        {
            await context.InsertManyAsync(list);
        }

        public async Task<UpdateResult> Increment(FilterDefinition<PromoCode> filter,
            UpdateDefinition<PromoCode> update)
        {
            UpdateResult res = await context.UpdateOneAsync(filter, update);
            return res;
        }


 public async Task<IEnumerable<PromoCode>> GetAll()
        {
            return await context
                .Find(_ => true)
                .ToListAsync();
        }
       

       

        public async Task<PromoCode> Find(Expression<Func<PromoCode, bool>> exp)
        {
            try
            {
                return await context
                    .Find(exp)
                    .FirstOrDefaultAsync();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return null;
            }
        }


        public async Task<long> Count(Expression<Func<PromoCode, bool>> exp)
        {
            return await context.CountDocumentsAsync(exp);
        }

        public async Task Create(PromoCode entity)
        {
            await context.InsertOneAsync(entity);
        }

        public async Task<PromoCode> FindAndUpdate(PromoCode PromoCode, UpdateDefinition<PromoCode> updateDefinition)
        {
            var filterDefinition = Builders<PromoCode>.Filter.Eq(p => p.Id, PromoCode.Id);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PromoCode>
            {
                ReturnDocument = ReturnDocument.After,
            };
            var playerData = await context.FindOneAndUpdateAsync(
                filterDefinition,
                updateDefinition,
                findOneAndUpdateOptions);

            return playerData;
        }


        public async Task<PromoCode> Update0(PromoCode entity)
        {
            //context.FindOneAndUpdate(p => p.Id == entity.Id, entity);

            var findOneAndUpdateOptions = new FindOneAndUpdateOptions<PromoCode>
            {
                ReturnDocument = ReturnDocument.After,
                // IsUpsert = true
            };
            FilterDefinition<PromoCode> filterDefinition = new ExpressionFilterDefinition<PromoCode>(p => p.Id == entity.Id);
            UpdateDefinition<PromoCode> updateDefinition = new ObjectUpdateDefinition<PromoCode>(entity);
            var data = await context.FindOneAndUpdateAsync(
                filterDefinition,
                updateDefinition,
                findOneAndUpdateOptions);

            if (data != null)
            {
                // The document already existed and was updated.
            }
            else
            {
                // The document did not exist and was inserted.
            }

            return data;



        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<PromoCode> filter = Builders<PromoCode>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await context
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}