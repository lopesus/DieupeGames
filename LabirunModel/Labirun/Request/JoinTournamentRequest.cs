using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class JoinTournamentRequest
    {
        public string Token { get; set; }
        public string LeaderBoardId { get; set; }
        public int LeaderBoardVersion { get; set; }
        public string TournamentCode { get; set; }
        public TournamentType TournamentType { get; set; }

        //post
        public long Score { get; set; }

        public JoinTournamentRequest()
        {
            
        }
        public JoinTournamentRequest(string token)
        {
            Token = token;
        }
    }
}