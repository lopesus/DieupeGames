using LiteDB;

namespace DieupeGames.Models.LiteDb
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}