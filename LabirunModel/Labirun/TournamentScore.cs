namespace LabirunModel.Labirun
{
    public partial class TournamentScore
    {
        public string leaderboardId { get; set; }
        public int versionId { get; set; }
        public string playerId { get; set; }
        public long score { get; set; }
        public object data { get; set; }
        public long createdAt { get; set; }
        public long updatedAt { get; set; }
        public string tCode { get; set; }
        public int tRank { get; set; }
        public long tClaimedAt { get; set; }
        public long tNotifiedAt { get; set; }
        public int previousScore { get; set; }
        public int rank { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.TournamentScore.score)}: {score}, {nameof(Labirun.TournamentScore.rank)}: {rank}, {nameof(Labirun.TournamentScore.leaderboardId)}: {leaderboardId}";
        }
    }
}