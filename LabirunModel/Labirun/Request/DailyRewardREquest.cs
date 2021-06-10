using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun.Request
{
    public class DailyRewardRequest : BasicRequest
    {
        // AdsType: AdsType;
        public RewardChestType RewardChestType { get; set; }

        public DailyRewardRequest()
        {

        }
        public DailyRewardRequest(string token) : base(token)
        {
        }
    }
}