using System.Collections.Generic;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Config
{
    public static class ItemPriceList
    {
        public static Dictionary<ItemId, long> PriceDico = new Dictionary<ItemId, long>()
        {
            {ItemId.FactoryMonthlyBoost200, 50000000},
            {ItemId.FactoryMonthlyBoost100, 30000000},
            {ItemId.FactoryMonthlyBoost50, 18000000},
            {ItemId.FactoryMonthlyBoost20, 9000000},


            {ItemId.FactoryDailyBoost100, 1200000},
            {ItemId.FactoryDailyBoost50, 720000},
            {ItemId.FactoryDailyBoost20, 432000},
            {ItemId.FactoryDailyBoost10, 216000},

            {ItemId.SpeedUp30Per10Min, 5000000},
            {ItemId.SpeedUp15Per10Min, 3000000},
            {ItemId.SpeedUp10Per10Min, 1800000},
            {ItemId.SpeedUp5Per10Min, 900000},

            {ItemId.SlowDown30Per10Min, 5000000},
            {ItemId.SlowDown15Per10Min, 3000000},
            {ItemId.SlowDown10Per10Min, 1800000},
            {ItemId.SlowDown5Per10Min, 900000},

            {ItemId.Afraid10SecPer10Min, 5000000},
            {ItemId.Afraid7SecPer10Min, 3000000},
            {ItemId.Afraid5SecPer10Min, 1800000},
            {ItemId.Afraid3SecPer10Min, 900000},
        };
    }
}