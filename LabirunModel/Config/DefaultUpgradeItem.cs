using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Config
{
    public static class DefaultUpgradeItem
    {

        public static ItemUpgrade HeroSpeedIncrease = new ItemUpgrade()
        {
            UpgradeId = UpgradeId.HeroSpeedIncrease,
            Unit = UpgradeUnit.Percent,
            Inc = 1,
            Val = 0,
            Count = 0,
            MaxCount = 30,
            Cost = 1000000,
            U0 = 1000000,
            Raison = 1000000,
        };

        public static ItemUpgrade AfraidTimeIncrease = new ItemUpgrade()
        {
            UpgradeId = UpgradeId.AfraidTimeIncrease,
            Unit = UpgradeUnit.Value,
            Inc = 0.1f,
            Val = 0,
            Count = 0,
            MaxCount = 50,
            Cost = 1000000,
            U0 = 1000000,
            Raison = 500000,
        };

        public static ItemUpgrade AfraidSpeedDecrease = new ItemUpgrade()
        {
            UpgradeId = UpgradeId.AfraidSpeedDecrease,
            Unit = UpgradeUnit.Percent,
            Inc = 1,
            Val = 0,
            Count = 0,
            MaxCount = 50,
            Cost = 1000000,
            U0 = 1000000,
            Raison = 100000,
        };

        public static ItemUpgrade FruitTimeIncrease = new ItemUpgrade()
        {
            UpgradeId = UpgradeId.FruitTimeIncrease,
            Unit = UpgradeUnit.Value,
            Inc = 0.1f,
            Val = 0,
            Count = 0,
            MaxCount = 50,
            Cost = 1000000,
            U0 = 1000000,
            Raison = 200000,
        };
        




    }
}