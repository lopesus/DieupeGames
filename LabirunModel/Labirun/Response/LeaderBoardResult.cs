using System.Collections.Generic;

namespace LabirunModel.Labirun.Response
{
    public class LeaderBoardResult
    {
        public LeaderBoardId LeaderBoardId { get; set; }
        public List<GlobalLeaderBoardEntry> First10 { get; set; }
        public GlobalLeaderBoardEntry PlayerEntry { get; set; }
        public List<GlobalLeaderBoardEntry> BeforePlayer { get; set; }
        public List<GlobalLeaderBoardEntry> AfterPlayer { get; set; }

        public LeaderBoardResult()
        {
            First10=new List<GlobalLeaderBoardEntry>();
            BeforePlayer=new List<GlobalLeaderBoardEntry>();
            AfterPlayer=new List<GlobalLeaderBoardEntry>();
        }
    }
}