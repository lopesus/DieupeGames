using System;
using System.Collections.Generic;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Config
{
    public static class SharedGlobalData
    {

        #region Max upgrades values

        public static int MaxHeroSpeedIncrease = 30;
        public static int MaxAfraidTimeIncrease = 10;

        public static int MaxAfraidSpeedDecrease = 10;
        public static int FruitTimeIncrease = 50;

        #endregion
        public static int MinuteInMillisec = 60000;
        public static int MaxInt32 = 2147483647;
        public static int HourInMillisec = 3600000;
        public static int DayInMillisec = 3600000 * 24;
        public static int ChallengeMazeCount = 100;
        public static int InitialMazeOpenCost = 10;

        public static int ProductInitialProductionPerHour = 10;

#if DEBUG
        public static int InitialCoin = 100000;
        public static int InitialGems = 300000000;
#else
        // production data
        public static int InitialCoin = 1000;
        public static int InitialGems = 10000000;
#endif


        public static int BankDeposit = 100000000;

        public static int InitialProductCount = 100;
        public static int InitialItemCount = 5;
        public static int MaxGemsPerLevel = 12000; //2000*6 max 6 enegerzier per maze in campaign
        public static int MaxCoinPerLevel = 80; //20 * 4;

        public static int CoinPerLevel = 10;

        public static int VideoRewardCoin = 50;
        public static int VideoRewardGems = 50000;
        public static int DailyRewardGems = 250000;

        public static int FruitToOpenMultiplier = 3;


        public static int InitialBankDailyPayout = 250000;
        public static int BankInterestIncrement = 1;
        //payout every 8 h
        public static int BankPayoutEvery = 24;
        public static int PromoBankDailyPayout = 250000;
        public static int GiveBankPayoutRewardAtLevel = 4; // 80;

        public static int MaxPromoBankDailyPayoutReward = 2; // 80;
        //public static int   GiveMonthlyRewardEvery = 2;

        public static int FruitNumber = 25;
        public static float BaseGoodPrice = 0.1f;

        public static int MaxCreatedMazePerPlayer = 5;


        public static AllSeeds AllSeeds = new AllSeeds()
        {
            ChallengeGemsMazeSeed = new Seed() { U1 = 337080, R = 57240, Alpha = 5, N = 25, Somme = 100112760 },
            PromoGemsMazeSeed = new Seed() { U1 = 33920, R = 5760, Alpha = 5, N = 25, Somme = 10074240 },
            EatGhostMazeSeed = new Seed() { U1 = 4455, R = 405, Alpha = 5, N = 25, Somme = 2456050 },
        };

        public static VideoReward DailyRewardItems = new VideoReward()
        {
            FixedReward = new List<GenericViewItem>()
            {
                new GenericViewItem()
                {
                    ItemId = ItemId.GemPack,
                    Count = DailyRewardGems
                },

                new GenericViewItem()
                {
                    ItemId = ItemId.CoinPack,
                    Count = VideoRewardCoin,
                    Type = ItemType.Chest,
                },

                new GenericViewItem()
                {
                    ItemId = ItemId.SpeedUp15Per10Min,
                    Count = 1,
                    Type = ItemType.HeroSpeedUp
                },
                new GenericViewItem()
                {
                    ItemId = ItemId.SlowDown15Per10Min,
                    Count = 1,
                    Type = ItemType.EnemySlowDown
                },
                new GenericViewItem()
                {
                    ItemId = ItemId.Afraid7SecPer10Min,
                    Count = 1,
                    Type = ItemType.EnemyAfraid
                },
            }
        };

        public static VideoReward SpeedUpVideoReward = new VideoReward()
        {
            FixedReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.GemPack, VideoRewardGems),
                new GenericViewItem(ItemId.CoinPack, VideoRewardCoin),
            },
            PickOneAtRandomReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.SpeedUp5Per10Min, true, 40),
                new GenericViewItem(ItemId.SpeedUp10Per10Min, true, 30),
                new GenericViewItem(ItemId.SpeedUp15Per10Min, true, 20),
                new GenericViewItem(ItemId.SpeedUp30Per10Min, true, 10),
            }
        };

        public static VideoReward SlowDownVideoReward = new VideoReward()
        {
            FixedReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.GemPack, VideoRewardGems),
                new GenericViewItem(ItemId.CoinPack, VideoRewardCoin),
            },
            PickOneAtRandomReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.SlowDown5Per10Min, true, 40),
                new GenericViewItem(ItemId.SlowDown10Per10Min, true, 30),
                new GenericViewItem(ItemId.SlowDown15Per10Min, true, 20),
                new GenericViewItem(ItemId.SlowDown30Per10Min, true, 10),
            }
        };

        public static VideoReward AfraidVideoReward = new VideoReward()
        {
            FixedReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.GemPack, VideoRewardGems),
                new GenericViewItem(ItemId.CoinPack, VideoRewardCoin),
            },
            PickOneAtRandomReward = new List<GenericViewItem>()
            {
                new GenericViewItem(ItemId.Afraid3SecPer10Min, true, 40),
                new GenericViewItem(ItemId.Afraid5SecPer10Min, true, 30),
                new GenericViewItem(ItemId.Afraid7SecPer10Min, true, 20),
                new GenericViewItem(ItemId.Afraid10SecPer10Min, true, 10),
            }
        };

        public static List<UpgradeItem> ProductProductionPerHourUpgrade = new List<UpgradeItem>()
        {
            new UpgradeItem() {Key = 1, Cost = 5, Increment = 1},
            new UpgradeItem() {Key = 2, Cost = 10, Increment = 1},
            new UpgradeItem() {Key = 3, Cost = 20, Increment = 1},
            new UpgradeItem() {Key = 4, Cost = 40, Increment = 1},
            new UpgradeItem() {Key = 5, Cost = 80, Increment = 1},
            new UpgradeItem() {Key = 6, Cost = 160, Increment = 1},
            new UpgradeItem() {Key = 7, Cost = 320, Increment = 3},
            new UpgradeItem() {Key = 8, Cost = 640, Increment = 5},
            new UpgradeItem() {Key = 9, Cost = 1280, Increment = 5},
            new UpgradeItem() {Key = 10, Cost = 2560, Increment = 8},
            new UpgradeItem() {Key = 11, Cost = 5120, Increment = 8},
            new UpgradeItem() {Key = 12, Cost = 10240, Increment = 10},
            new UpgradeItem() {Key = 13, Cost = 20480, Increment = 10},
            new UpgradeItem() {Key = 14, Cost = 40960, Increment = 10},
            new UpgradeItem() {Key = 15, Cost = 81920, Increment = 12},
            new UpgradeItem() {Key = 16, Cost = 163840, Increment = 13},
            new UpgradeItem() {Key = 17, Cost = 327680, Increment = 15},
            new UpgradeItem() {Key = 18, Cost = 655360, Increment = 20},
            new UpgradeItem() {Key = 19, Cost = 1310720, Increment = 25},
            new UpgradeItem() {Key = 20, Cost = 2621440, Increment = 50},
        };


        public static List<UpgradeItem> ProductionLineUpgrade = new List<UpgradeItem>()
        {
            new UpgradeItem() {Key = 1, Cost = 500000, Increment = 2},
            new UpgradeItem() {Key = 2, Cost = 600000, Increment = 2},
            new UpgradeItem() {Key = 3, Cost = 700000, Increment = 2},
            new UpgradeItem() {Key = 4, Cost = 800000, Increment = 2},
            new UpgradeItem() {Key = 5, Cost = 900000, Increment = 2},
            new UpgradeItem() {Key = 6, Cost = 1000000, Increment = 2},
            new UpgradeItem() {Key = 7, Cost = 1100000, Increment = 8},
            new UpgradeItem() {Key = 8, Cost = 1200000, Increment = 10},
            new UpgradeItem() {Key = 9, Cost = 1300000, Increment = 20},
            new UpgradeItem() {Key = 10, Cost = 2000000, Increment = 50},
        };


        public static List<UpgradeItem> FactoryUpgrade = new List<UpgradeItem>()
        {
            new UpgradeItem() {Key = 1, Cost = 10000000, Increment = 10},
            new UpgradeItem() {Key = 2, Cost = 14000000, Increment = 15},
            new UpgradeItem() {Key = 3, Cost = 18000000, Increment = 20},
            new UpgradeItem() {Key = 4, Cost = 22000000, Increment = 25},
            new UpgradeItem() {Key = 5, Cost = 26000000, Increment = 30},
            new UpgradeItem() {Key = 6, Cost = 30000000, Increment = 35},
            new UpgradeItem() {Key = 7, Cost = 34000000, Increment = 40},
            new UpgradeItem() {Key = 8, Cost = 38000000, Increment = 45},
            new UpgradeItem() {Key = 9, Cost = 42000000, Increment = 60},
            new UpgradeItem() {Key = 10, Cost = 46000000, Increment = 120},
        };


        public static List<ProductionRatio> GoodFactoryProductionRatio = new List<ProductionRatio>()
        {
            new ProductionRatio {Key = ProductionTime.Hour1, Val = 100},
            new ProductionRatio {Key = ProductionTime.Hour4, Val = 167},
            new ProductionRatio {Key = ProductionTime.Hour8, Val = 250},
            new ProductionRatio {Key = ProductionTime.Hour24, Val = 500}
        };

        public static List<ItemId> ProductItemsList = new List<ItemId>()
        {
            ItemId.Fruit1,
            ItemId.Fruit2,
            ItemId.Fruit3,
            ItemId.Fruit4,
            ItemId.Fruit5,
            ItemId.Fruit6,
            ItemId.Fruit7,
            ItemId.Fruit8,
            ItemId.Fruit9,
            ItemId.Fruit10,
            ItemId.Fruit11,
            ItemId.Fruit12,
            ItemId.Fruit13,
            ItemId.Fruit14,
            ItemId.Fruit15,
            ItemId.Fruit16,
            ItemId.Fruit17,
            ItemId.Fruit18,
            ItemId.Fruit19,
            ItemId.Fruit20,
            ItemId.Fruit21,
            ItemId.Fruit22,
            ItemId.Fruit23,
            ItemId.Fruit24,
            ItemId.Fruit25,
        };

        public static List<ItemId> MealsItemsList = new List<ItemId>()
        {
            ItemId.Meal1,
            ItemId.Meal2,
            ItemId.Meal3,
            ItemId.Meal4,
            ItemId.Meal5,
            ItemId.Meal6,
            ItemId.Meal7,
            ItemId.Meal8,
            ItemId.Meal9,
            ItemId.Meal10,
        };

        public static List<ItemId> FactoryDailyBoostItemsList = new List<ItemId>()
        {
            ItemId.FactoryDailyBoost10,
            ItemId.FactoryDailyBoost20,
            ItemId.FactoryDailyBoost50,
            ItemId.FactoryDailyBoost100,
        };

        public static List<ItemId> FactoryMonthlyBoostItemsList = new List<ItemId>()
        {
            ItemId.FactoryMonthlyBoost20,
            ItemId.FactoryMonthlyBoost50,
            ItemId.FactoryMonthlyBoost100,
            ItemId.FactoryMonthlyBoost200,
        };

        public static List<ItemId> SpeedUpItemsList = new List<ItemId>()
        {
            ItemId.SpeedUp5Per10Min,
            ItemId.SpeedUp10Per10Min,
            ItemId.SpeedUp15Per10Min,
            ItemId.SpeedUp30Per10Min,
        };


        public static List<ItemId> SlowDownItemsList = new List<ItemId>()
        {
            ItemId.SlowDown5Per10Min,
            ItemId.SlowDown10Per10Min,
            ItemId.SlowDown15Per10Min,
            ItemId.SlowDown30Per10Min,
        };


        public static List<ItemId> AfraidItemsList = new List<ItemId>()
        {
            ItemId.Afraid3SecPer10Min,
            ItemId.Afraid5SecPer10Min,
            ItemId.Afraid7SecPer10Min,
            ItemId.Afraid10SecPer10Min,
        };

        public static List<ItemId> ChestItemsList = new List<ItemId>()
        {
            ItemId.SlowDownChest,
            ItemId.SpeedUpChest,
            ItemId.AfraidChest,
        };


        public static float GetItemPrice(ItemId itemId)
        {
            var index = ProductItemsList.IndexOf(itemId);
            if (index >= 0)
            {
                return (index + 1) * BaseGoodPrice;
            }

            return 0;
        }


        public static int GetBoostValue(ItemId itemId)
        {
            switch (itemId)
            {
                case ItemId.FactoryDailyBoost10:
                    return 10;
                case ItemId.FactoryDailyBoost20:
                    return 20;
                case ItemId.FactoryDailyBoost50:
                    return 50;
                case ItemId.FactoryDailyBoost100:
                    return 100;
                case ItemId.FactoryMonthlyBoost20:
                    return 20;
                case ItemId.FactoryMonthlyBoost50:
                    return 50;
                case ItemId.FactoryMonthlyBoost100:
                    return 100;
                case ItemId.FactoryMonthlyBoost200:
                    return 200;

                case ItemId.SpeedUp5Per10Min:
                    return 5;
                case ItemId.SpeedUp10Per10Min:
                    return 10;
                case ItemId.SpeedUp15Per10Min:
                    return 15;
                case ItemId.SpeedUp30Per10Min:
                    return 30;
                case ItemId.SlowDown5Per10Min:
                    return 5;
                case ItemId.SlowDown10Per10Min:
                    return 10;
                case ItemId.SlowDown15Per10Min:
                    return 15;
                case ItemId.SlowDown30Per10Min:
                    return 30;
                case ItemId.Afraid3SecPer10Min:
                    return 3;
                case ItemId.Afraid5SecPer10Min:
                    return 5;
                case ItemId.Afraid7SecPer10Min:
                    return 7;
                case ItemId.Afraid10SecPer10Min:
                    return 10;
                default:
                    return 0;
            }
        }


        public static int GetBankPayoutBonus(int level)
        {
            switch (level)
            {
                case 1: return 500;
                case 2: return 1000;
                case 3: return 1500;
                case 4: return 2000;
                case 5: return 2500;
                default: return 0;
            }
        }

        public static int GetProductionTimeInHour(ProductionTime time)
        {
            switch (time)
            {
                case ProductionTime.Hour1:
                    return 1;
                case ProductionTime.Hour4:
                    return 4;
                case ProductionTime.Hour8:
                    return 8;
                case ProductionTime.Hour24:
                    return 24;
                default:
                    return 24;
            }
        }
    }
}