using System;
using System.Collections.Generic;

namespace LabirunModel.Labirun
{
    public class CreatedMaze
    {
        //global maze id , assigned by db
        public string Id { get; set; }
        public long SystemName { get; set; }
        public string Name { get; set; }
        public string CreatorName { get; set; }

        public string PlayerId { get; set; }

        public bool IsPublic { get; set; }

        public DateTime UpdateTime { get; set; }

        public int DimX { get; set; }
        public int DimY { get; set; }

        //comma separated list of tile
        public string Data { get; set; }
        public string Image { get; set; }

        public long PlayCount { get; set; }
        public int Rating { get; set; }

        public List<MazeRunScore> BestScores { get; set; }
        public long BestScore { get; set; }
        public string BestPlayer { get; set; }
        public DateTime BestDate { get; set; }

        public long FastestScore { get; set; }
        public string FastestPlayer { get; set; }
        public DateTime FastDate { get; set; }

        public CreatedMaze()
        {
            BestScores=new List<MazeRunScore>();
        }

        public void UpdateStats(CreatedMaze maze)
        {
            if (maze == null) return;

            this.BestScore = maze.BestScore;
            this.BestPlayer = maze.BestPlayer;
            this.BestDate = maze.BestDate;

            this.FastestScore = maze.FastestScore;
            this.FastestPlayer = maze.FastestPlayer;
            this.FastDate = maze.FastDate;

            this.PlayCount = maze.PlayCount;
            this.Rating = maze.Rating;
        }
        public override string ToString()
        {
            return
                $" {SystemName} - {CreatorName} - {nameof(IsPublic)}: {IsPublic}";
        }
        public  string Dump()
        {
            return $"{nameof(SystemName)}: {SystemName} {nameof(CreatorName)}: {CreatorName}  {nameof(IsPublic)}: {IsPublic}" +
                   $"\r\n {nameof(PlayCount)}: {PlayCount}  {nameof(Rating)}: {Rating}" +
                   $"\r\n {nameof(BestScore)}: {BestScore} {nameof(BestPlayer)}: {BestPlayer}" +
                   $"\r\n {nameof(FastestScore)}: {FastestScore} {nameof(FastestPlayer)}: {FastestPlayer}";
        }
    }
}