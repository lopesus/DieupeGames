using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Helpers;
using DieupeGames.Services;
using LabirunModel.Config;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;
using LabirunModel.Labirun.Request;
using LabirunModel.Labirun.Response;
using LabirunModel.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DieupeGames.Controllers.PacRun
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public AdsController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }



        [HttpPost("AddDailyReward")]
        public async Task<ApiResponse<PlayerData>> AddDailyReward([FromBody]DailyRewardRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                if (player.NextDailyRewardTime.IsExpired())
                {
                    // playerData.NextDailyRewardTime = DateTime.UtcNow.AddDays(1);
                    var nextTime = DateTime.UtcNow.AddHours(2);
                    var update = Builders<PlayerData>.Update.
                            Set(p => p.NextDailyRewardTime, nextTime);
                    var reward = SharedGlobalData.DailyRewardItems;
                    foreach (var item in reward.FixedReward)
                    {
                        switch (item.ItemId)
                        {
                            case ItemId.CoinPack:
                                update = update.Inc(p => p.VirtualCurrencies.Coin, item.Count);
                                break;
                            case ItemId.GemPack:
                                update = update.Inc(p => p.VirtualCurrencies.Gems, item.Count);
                                break;
                            default:
                                player.AddToInventoryItem(item);
                                break;
                        }
                        // playerData = await context.Players.UpdateInventoryItem(playerData.Id, item.ItemId, item.Count);

                    }

                    update = update.Set(p => p.Inventory, player.Inventory);

                    player = await context.Players.FindAndUpdate(player, update);
                    // playerData = await context.PlayerData.Update(playerData);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        Inventory = player.Inventory,
                        Bank = player.Bank,
                        NextDailyRewardTime = nextTime
                    });
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

        [HttpPost("AddVideoReward")]
        public async Task<ApiResponse<PlayerData>> AddVideoReward([FromBody]DailyRewardRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                List<ItemProbability> chest;
                if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

                VideoReward reward;


                switch (request.RewardChestType)
                {
                    case RewardChestType.SpeedUpChest:
                        reward = SharedGlobalData.SpeedUpVideoReward;
                        break;
                    case RewardChestType.SlowDownChest:
                        reward = SharedGlobalData.SlowDownVideoReward;
                        break;
                    case RewardChestType.AfraidChest:
                        reward = SharedGlobalData.AfraidVideoReward;
                        break;
                    default:
                        return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
                }


                var update = Builders<PlayerData>.Update.
                    Set(p => p.LastLogin, DateTime.UtcNow);

                foreach (var item in reward.FixedReward)
                {
                    switch (item.ItemId)
                    {
                        case ItemId.CoinPack:
                            update = update.Inc(p => p.VirtualCurrencies.Coin, item.Count);
                            break;
                        case ItemId.GemPack:
                            update = update.Inc(p => p.VirtualCurrencies.Gems, item.Count);
                            break;
                    }
                }
               
                var picked = CommonTools.OpenChest(reward.PickOneAtRandomReward);
                player.AddToInventoryItem(picked,1);

                update = update.Set(p => p.Inventory, player.Inventory);

                player = await context.Players.FindAndUpdate(player, update);



                //return only modified data
                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {
                    VirtualCurrencies = player.VirtualCurrencies,
                    Inventory = player.Inventory,
                    Bank = player.Bank,
                    //send picked item back
                    ServerResponse = new ServerResponse() { PickedItem = picked }
                });

            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }
    }
}