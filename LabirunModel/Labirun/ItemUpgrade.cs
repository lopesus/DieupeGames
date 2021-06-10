using LabirunModel.Labirun.Enums;
using LabirunModel.Tools;

namespace LabirunModel.Labirun
{
    public partial class ItemUpgrade
    {
        //public long Id { get; set; }
        public UpgradeId UpgradeId { get; set; }
        public UpgradeUnit Unit { get; set; }
        public float Inc { get; set; }
        public float Val { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public int Cost { get; set; }
        public int U0 { get; set; }
        public int Raison { get; set; }

        public ItemUpgrade ShallowCopy()
        {
            return (ItemUpgrade)this.MemberwiseClone();
        }

        public void BoostTest(int counter)
        {
            Count = counter;
            Val = Inc * counter;

            var suite = new SuiteArithmetique1(U0, Raison);
            Cost = (int)suite.Terme(Count);
        }

        public void Unlock()
        {
            Val += Inc;
            Count++;
            //var suite = new SuiteArithmetique(U0, Raison);
            Cost += Raison; // suite.Terme(Counter);
        }

        public void Lock()
        {
            Val -= Inc;
            Count--;
            //var suite = new SuiteArithmetique(U0, Raison);
            Cost -= Raison; // suite.Terme(Counter);
        }

        public override string ToString()
        {
            return
                $"{nameof(Labirun.ItemUpgrade.UpgradeId)}: {UpgradeId}, {nameof(Labirun.ItemUpgrade.Unit)}: {Unit}, {nameof(Labirun.ItemUpgrade.Inc)}: {Inc}, {nameof(Labirun.ItemUpgrade.Val)}: {Val}, {nameof(Labirun.ItemUpgrade.Count)}: {Count}, {nameof(Labirun.ItemUpgrade.MaxCount)}: {MaxCount}, {nameof(Labirun.ItemUpgrade.Cost)}: {Cost}, {nameof(Labirun.ItemUpgrade.U0)}: {U0}, {nameof(Labirun.ItemUpgrade.Raison)}: {Raison}";
        }
    }
}