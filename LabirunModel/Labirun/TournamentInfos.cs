using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class TournamentInfos
    {
        //public long Id { get; set; }
        public string LeaderBoardId { get; set; }
        public TournamentType Type { get; set; }
        public int LeaderBoardVersion { get; set; }
        public bool Enrolled { get; set; }
        public bool Completed { get; set; }
        public bool HasPostedScore { get; set; }
        public long PlayStartTime { get; set; }
        public long PlayEndTime { get; set; }

        public long RegistrationStartTime { get; set; }
        public long RegistrationEndTime { get; set; }
        public string State { get; set; }

        // if < to LeaderBoardVersion then tournament completed
        //LastPostVersionId?: number{ get; set; }

        public TournamentScore TournamentScore { get; set; }


        public override string ToString()
        {
            return $"{LeaderBoardId}";
        }

        public string ToString2()
        {
            return $"{nameof(Labirun.TournamentInfos.LeaderBoardId)}: {LeaderBoardId},\r\n " +
                   $"{nameof(Labirun.TournamentInfos.Type)}: {Type},\r\n {nameof(Labirun.TournamentInfos.LeaderBoardVersion)}: {LeaderBoardVersion}," +
                   $"\r\n {nameof(Labirun.TournamentInfos.Enrolled)}: {Enrolled},\r\n" +
                   $" {nameof(Labirun.TournamentInfos.PlayStartTime)}: {PlayStartTime}, \r\n" +
                   $"{nameof(Labirun.TournamentInfos.PlayEndTime)}: {PlayEndTime}, \r\n" +
                   $"{nameof(Labirun.TournamentInfos.RegistrationStartTime)}: {RegistrationStartTime},\r\n" +
                   $" {nameof(Labirun.TournamentInfos.RegistrationEndTime)}: {RegistrationEndTime},\r\n" +
                   $" {nameof(Labirun.TournamentInfos.LeaderBoardVersion)}: {LeaderBoardVersion}";
        }
    }
}