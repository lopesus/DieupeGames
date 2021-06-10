using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DieupeGames.Data.Mongo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
       // Task<T> Find(string name);
        Task<T> Find(Expression<Func<T, bool>> exp);
        Task<long> Count(Expression<Func<T, bool>> exp);
        Task Create(T entity);

        Task<T> FindAndUpdate(T entity,UpdateDefinition<T> update);
        //Task<T> Update(T entity,UpdateDefinition<T> update);
        Task<bool> Delete(string id);
        Task SaveList(List<T> list);

        Task<UpdateResult> Increment(FilterDefinition<T> filter, UpdateDefinition<T> update);
        //{
        //    //var filter = Builders<PlayerData>.Filter.Eq(p => p.Id, player.Id);
        //    //var update = Builders<PlayerData>.Update.Inc(p => p.VirtualCurrencies.Gems, 10);

        //}

    }
}