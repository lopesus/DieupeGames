using System.Collections.Generic;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public static class ShopProductsCode
    {
        public static Dictionary<ShopItemId, string> ProductsList = new Dictionary<ShopItemId, string>()
        {
            {ShopItemId.LegendaryStart, "gems_promo"},

            //gems
            {ShopItemId.LotOfGems, "gems_standard"},
            {ShopItemId.LotOfGems20, "gems_standard20"},
            {ShopItemId.LotOfGems30, "gems_standard30"},
            {ShopItemId.LotOfGems40, "gems_standard40"},
            {ShopItemId.LotOfGems60, "gems_standard60"},
            {ShopItemId.LotOfGems100, "gems_standard100"},

            //montly 
            {ShopItemId.MonthlyProductionBoost100, "factory_monthly100"},
            {ShopItemId.MonthlyProductionBoost200, "factory_monthly200"},
            {ShopItemId.MonthlyProductionBoost300, "factory_monthly300"},

            //speed up 
            {ShopItemId.SlowDownChest, "slowdown_chest"},
            {ShopItemId.SpeedUpChest, "speedup_chest"},
            {ShopItemId.AfraidChest, "afraid_chest"},
        };
    }
}