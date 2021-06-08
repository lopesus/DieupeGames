//using System.Linq;
//using LabirunModel.Labirun;
//using LabirunModel.Labirun.Enums;
//using MongoDB.Driver;

//namespace LabirunServer.Helpers
//{
//    public static class MongoDbTools
//    {
//        public static FilterDefinition<PlayerData> CreateChallengeFilter(string playerId, int mazeId)
//        {
//            FilterDefinition<PlayerData> filter;
//            filter = Builders<PlayerData>.Filter.Where(
//                p => p.Id == playerId
//                     && p.ChallengeMazeList.Any(i => i.MazeId == mazeId));
//            return filter;
//        }


//        public static FilterDefinition<PlayerData> CreateChallengeFilter(string playerId, int mazeId)
//        {
//            FilterDefinition<PlayerData> filter;
//            filter = Builders<PlayerData>.Filter.Where(
//                p => p.Id == playerId
//                     && p.ChallengeMazeList.Any(i => i.MazeId == mazeId));
//            return filter;
//        }


//        public static FilterDefinition<PlayerData> CreateInventoryFilter(string playerId, ItemId itemId)
//        {
//            FilterDefinition<PlayerData> filter;
//            switch (itemId)
//            {
//                case ItemId.GemPack:
//                case ItemId.CoinPack:
//                    filter = Builders<PlayerData>.Filter.Where(p => p.Id == playerId);
//                    return filter;

//                default:
//                    filter = Builders<PlayerData>.Filter.Where(
//                        p => p.Id == playerId
//                             && p.Inventory.AllItems.Any(i => i.ItemId == itemId));
//                    return filter;
//            }
//        }

//        public static UpdateDefinition<PlayerData> CreateInventoryUpdate(ItemId itemId, int count)
//        {
//            UpdateDefinition<PlayerData> update;
//            switch (itemId)
//            {
//                case ItemId.GemPack:
//                    update = Builders<PlayerData>.Update.Inc(p => p.VirtualCurrencies.Gems, count);
//                    return update;
//                case ItemId.CoinPack:
//                    update = Builders<PlayerData>.Update.Inc(p => p.VirtualCurrencies.Coin, count);
//                    return update;

//                default:
//                    update = Builders<PlayerData>.Update.Inc(x => x.Inventory.AllItems[-1].Count, count);
//                    return update;
//            }
//        }


//        //public static UpdateDefinition<PlayerData> AddToInventoryItem(UpdateDefinition<PlayerData> update, InventoryItem item)
//        //{
//        //    switch (item.ItemId)
//        //    {
//        //        case ItemId.CoinPack:
//        //            update = update.Inc(p => p.VirtualCurrencies.Coin, item.Count);
//        //            VirtualCurrencies.Coin += item.Count;
//        //            break;
//        //        case ItemId.GemPack:
//        //            update = update.Inc(p => p.VirtualCurrencies.Gems, item.Count);
//        //            VirtualCurrencies.Gems += item.Count;
//        //            break;
//        //        default:
//        //            var invItem = Inventory.AllItems.FirstOrDefault(i => i.ItemId == item.ItemId);
//        //            if (invItem != null) invItem.Count += item.Count;
//        //            break;
//        //    }

//        //    return update;
//        //}

//    }
//}