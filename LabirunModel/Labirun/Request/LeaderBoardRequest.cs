using System.Collections.Generic;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
   
    public class LeaderBoardRequest
    {
        public string Token { get; set; }
        public LeaderBoardId LeaderBoardId { get; set; }
        public int Score { get; set; }
        public List<GlobalLeaderBoardEntry> InitialEntryList { get; set; }

        //public int LeaderBoardVersion { get; set; }
        //string TournamentCode { get; set; }
        //TournamentType TournamentType { get; set; }

        public LeaderBoardRequest()
        {
            
        }
        public LeaderBoardRequest(string token)
        {
            Token = token;
        }
    }
}

