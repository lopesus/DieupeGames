namespace LabirunModel.Labirun
{
    public partial class GameStatistics
    {
        //specific to a player
        public int TotalPoint { get; set; }
        public int GemsChallenge { get; set; }


        //promo
        public int PromoCount { get; set; }

        public long PromoBankPayout { get; set; }
        public int PromoBankPayoutCount { get; set; }
        public int PromoGems { get; set; }

        //Tournament
        public int TournamentFastest { get; set; }

        public override string ToString()
        {
            return $"{nameof(TotalPoint)}: {TotalPoint}\r\n " +
                   $"{nameof(PromoCount)}: {PromoCount}\r\n " +
                   $"{nameof(PromoBankPayout)}: {PromoBankPayout}\r\n " +
                   $"{nameof(PromoBankPayoutCount)}: {PromoBankPayoutCount}\r\n " +
                   $"{nameof(PromoGems)}: {PromoGems}\r\n " +
                   $"{nameof(GemsChallenge)}: {GemsChallenge}\r\n " +
                   $"{nameof(TournamentFastest)}: {TournamentFastest}";
        }
    }
}