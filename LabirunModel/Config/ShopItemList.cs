using System.Collections.Generic;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Config
{
    public static class ShopItemList
    {
        //public static Dictionary<BindingId, ShopItemGroup> ShopItemGroups;
        static ShopItem LegendaryStart;
        static ShopItem LotOfGems;
        static ShopItem LotOfGems40;
        static ShopItem LotOfGems100;
        static ShopItem MontlyProductionBoost100;
        static ShopItem MontlyProductionBoost200;
        static ShopItem MontlyProductionBoost300;
        static ShopItem SlowDownChest;
        static ShopItem SpeedUpChest;
        static ShopItem AfraidChest;

        public static List<ShopItem> ItemList = new List<ShopItem>();

        static ShopItemList()
        {
            LegendaryStart = new ShopItem(ShopItemId.LegendaryStart, 200000000)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.GemPack, 200000000),
                    new Payout(ItemId.SpeedUp30Per10Min, 50),
                    new Payout(ItemId.SlowDown30Per10Min, 50),
                    new Payout(ItemId.Afraid10SecPer10Min, 50),

                }
            };

            LotOfGems = new ShopItem(ShopItemId.LotOfGems, 10000000)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.GemPack, 10000000)
                }
            };

            LotOfGems40 = new ShopItem(ShopItemId.LotOfGems40, LotOfGems, 8, 40);
            LotOfGems100 = new ShopItem(ShopItemId.LotOfGems100, LotOfGems, 32, 100);

            MontlyProductionBoost100 = new ShopItem(ShopItemId.MonthlyProductionBoost100, 1)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.FactoryMonthlyBoost200, 4),
                    new Payout(ItemId.FactoryDailyBoost100, 4 * 3),
                    new Payout(ItemId.FactoryDailyBoost50, 4 * 7),
                    new Payout(ItemId.FactoryDailyBoost20, 4 * 20),
                }
            };

            MontlyProductionBoost200 = new ShopItem(ShopItemId.MonthlyProductionBoost200, 1)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.FactoryMonthlyBoost200, 4),
                    new Payout(ItemId.FactoryDailyBoost100, 60),
                    new Payout(ItemId.FactoryDailyBoost50, 60),
                }
            };
            MontlyProductionBoost300 = new ShopItem(ShopItemId.MonthlyProductionBoost300, 1)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.FactoryMonthlyBoost200, 4),
                    new Payout(ItemId.FactoryDailyBoost100, 120),
                }
            };

            SlowDownChest = new ShopItem(ShopItemId.SlowDownChest, 75)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.SlowDown5Per10Min, 30),
                    new Payout(ItemId.SlowDown10Per10Min, 20),
                    new Payout(ItemId.SlowDown15Per10Min, 15),
                    new Payout(ItemId.SlowDown30Per10Min, 10),
                }
            };
            SpeedUpChest = new ShopItem(ShopItemId.SpeedUpChest, 75)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.SpeedUp5Per10Min, 30),
                    new Payout(ItemId.SpeedUp10Per10Min, 20),
                    new Payout(ItemId.SpeedUp15Per10Min, 15),
                    new Payout(ItemId.SpeedUp30Per10Min, 10),
                }
            };
            AfraidChest = new ShopItem(ShopItemId.AfraidChest, 75)
            {
                PayoutList = new List<Payout>()
                {
                    new Payout(ItemId.Afraid3SecPer10Min, 30),
                    new Payout(ItemId.Afraid5SecPer10Min, 20),
                    new Payout(ItemId.Afraid7SecPer10Min, 15),
                    new Payout(ItemId.Afraid10SecPer10Min, 10),
                }
            };

            ItemList = new List<ShopItem>()
            {
                LegendaryStart,
                LotOfGems,
                LotOfGems40,
                LotOfGems100,
                MontlyProductionBoost100,
                MontlyProductionBoost200,
                MontlyProductionBoost300,
                SlowDownChest,
                SpeedUpChest,
                AfraidChest,
            };
        }


        public static ShopItemGroup GetShopItemGroup(HeaderId bindingId)
        {
            var ShopItemGroups = GetGroups();
            if (ShopItemGroups != null)
            {
                if (ShopItemGroups.ContainsKey(bindingId))
                {
                    return ShopItemGroups[bindingId];
                }
            }

            return null;
        }

        public static Dictionary<HeaderId, ShopItemGroup> GetGroups()
        {
            var shopItemGroups = new Dictionary<HeaderId, ShopItemGroup>();
            shopItemGroups[HeaderId.ShopLegendaryStartHeader] = new ShopItemGroup()
            {
                HeaderBindingId = HeaderId.ShopLegendaryStartHeader,
                ShopItemsList = new List<ShopItem>()
                {
                    LegendaryStart,
                },
            };

            shopItemGroups[HeaderId.ShopGemsHeader] = new ShopItemGroup()
            {
                HeaderBindingId = HeaderId.ShopGemsHeader,
                ShopItemsList = new List<ShopItem>()
                {
                    LotOfGems, //LotOfGems20,LotOfGems30,
                    LotOfGems40,
                    //LotOfGems60,
                    LotOfGems100
                },
            };

            shopItemGroups[HeaderId.ShopFactoryMonthlyHeader] =
                new ShopItemGroup()
                {
                    HeaderBindingId = HeaderId.ShopFactoryMonthlyHeader,
                    ShopItemsList = new List<ShopItem>()
                    {
                        MontlyProductionBoost100,
                        MontlyProductionBoost200,
                        MontlyProductionBoost300,
                    },
                };

            shopItemGroups[HeaderId.ShopSpeedUpHeader] =
                new ShopItemGroup()
                {
                    HeaderBindingId = HeaderId.ShopSpeedUpHeader,
                    ShopItemsList = new List<ShopItem>()
                    {
                        SpeedUpChest, SlowDownChest, AfraidChest,
                    },
                };

            return shopItemGroups;
        }
    }
}