using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class CreatedMazeRequest
    {
        public string Id { get; set; }
        public long SystemName { get; set; }

        public string UserName { get; set; }

        public long BestScore { get; set; }
        public long FastestScore { get; set; }
        public MazePlayStyle PlayStyle { get; set; }

        public CreatedMazeRequest()
        {
            BestScore = 0;
            FastestScore = long.MaxValue;
        }
    }
    
}