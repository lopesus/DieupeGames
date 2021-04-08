using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace DieupeGames.Models.LiteDb
{
    public interface ILiteDbWordBoxService
    {
       // bool Delete(string id);
        IEnumerable<Word> FindAll();
        Word FindOne(string id);
        bool Insert(Word forecast);
        bool Update(Word forecast);
    }

    public class LiteDbWordBoxService : ILiteDbWordBoxService
    {

        private LiteDatabase liteDb;

        public LiteDbWordBoxService(ILiteDbContext liteDbContext)
        {
            liteDb = liteDbContext.Database;
        }

        public IEnumerable<Word> FindAll()
        {
            var result = liteDb.GetCollection<Word>("WordBox")
                .FindAll();
            return result;
        }

        public Word FindOne(string id)
        {
            return liteDb.GetCollection<Word>("WordBox")
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public bool Insert(Word forecast)
        {
            return liteDb.GetCollection<Word>("WordBox")
                .Insert(forecast);
        }

        public bool Update(Word forecast)
        {
            return liteDb.GetCollection<Word>("WordBox")
                .Update(forecast);
        }

        //public bool Delete(string id)
        //{
        //    return _liteDb.GetCollection<WordBox>("WordBox")
        //        .Delete(x => x.Id == id);
        //}
    }
}