namespace LabirunModel.Labirun.Request
{
    public class ChallengeProgressRequest
    {
        public ChallengeProgressRequest()
        {
            
        }
        public ChallengeProgressRequest(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
        public int MazeId { get; set; }
        public int MazeLevel { get; set; }


        // Coin: number{ get; set; }
        //Energy:number{ get; set; }
        //  Gems: number{ get; set; }
        public int LevelFruit { get; set; }
        public int TotalPoint { get; set; }
        public long TimeInMilliSec { get; set; }

        //eat  4 ghost with one energizer
        public int Combo4Ghost { get; set; }

        //eat all 4 ghost with one energizer , for each energizer in a level
        public int SuperCombo4 { get; set; }
    }
}