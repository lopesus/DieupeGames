//using System.Collections.Generic;
//using System.Linq;
//using LabirunModel.Labirun;

//namespace LabirunServer.Services
//{

//    //public interface ILeaderBoardCacheService
//    //{
//    //    void Update(GlobalLeaderBoardEntry entry);
//    //}

//    //public class LeaderBoardCacheService : ILeaderBoardCacheService
//    //{
//    //    public IndexedList IndexedList { get; set; }
//    //    public Dictionary<string, GlobalLeaderBoardEntry> DicoEntries { get; set; }

//    //    //public List<GlobalLeaderBoardEntry> LeaderBoardEntries { get; set; }

//    //    public LeaderBoardCacheService()
//    //    {
//    //        //LeaderBoardEntries = new List<GlobalLeaderBoardEntry>();
//    //        DicoEntries = new Dictionary<string, GlobalLeaderBoardEntry>();
//    //    }

//    //    public void Initialize(List<GlobalLeaderBoardEntry> list)
//    //    {
//    //        list.Sort(new TotalPointComparer());
//    //        //list = list.OrderByDescending(b => b.TotalPoint.Score).ToList();
//    //        DicoEntries = list.ToDictionary(e => e.Id, e => e);
//    //        //DicoEntries = DicoEntries.OrderByDescending(e => e.Value.TotalPoint.Rank);
//    //    }
//    //    public void Update(GlobalLeaderBoardEntry entry)
//    //    {
//    //        DicoEntries[entry.Id] = entry;
//    //    }

//    //    public GlobalLeaderBoardEntry GetRank(string id)
//    //    {
//    //        DicoEntries.TryGetValue(id, out var entry);
//    //        return entry;
//    //    }

//    }

//}