using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class Payout
    {
        public ItemId ItemId;
        public long Amount;

        public Payout(ItemId id, long amount)
        {
            ItemId = id;
            Amount = amount;
        }
    }

   
}