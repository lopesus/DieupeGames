using System.Collections.Generic;
using LabirunModel.Labirun.Enums;

namespace LabirunModel.Labirun
{
    public partial class ShopItem
    {
        public ShopItemId ShopItemId { get; set; }
        public string StoreCode { get; set; }
        public int Amount { get; set; }
        public List<Payout> PayoutList { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string LocalPriceString { get; set; }


        public ShopItem(ShopItemId shopItemId, int amount)
        {
            ShopItemId = shopItemId;
            Amount = amount;
            StoreCode = ShopProductsCode.ProductsList[shopItemId];
            PayoutList = new List<Payout>();
        }

        public ShopItem(ShopItemId shopItemId, ShopItem source, int multiplier, int incPercent)
        {
            ShopItemId = shopItemId;
            StoreCode = ShopProductsCode.ProductsList[shopItemId];

            long val = source.Amount;
            val = val * multiplier;
            var inc = val * incPercent / 100;
            val += inc;
            Amount = (int)val;

            PayoutList = new List<Payout>();
            foreach (var pay in source.PayoutList)
            {
                var key = pay.ItemId;
                val = pay.Amount;
                val = val * multiplier;
                inc = val * incPercent / 100;
                val += inc;

                PayoutList.Add(new Payout(pay.ItemId, val));
            }
        }


    }
}