using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Helpers;
using DieupeGames.Services;
using LabirunModel.Config;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Request;
using LabirunModel.Labirun.Response;
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
    public class MarketController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public MarketController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }


        [HttpPost("BuyItem")]
        public async Task<ApiResponse<PlayerData>> BuyItem([FromBody]BuyItemRequest request)
        {
             var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var playerId = player.Id;
                var item = player.Inventory.AllItems.FirstOrDefault(i => i.ItemId == request.Id);
                if (item != null && ItemPriceList.PriceDico.ContainsKey(item.ItemId))
                {
                    var price = ItemPriceList.PriceDico[item.ItemId];
                    if (player.VirtualCurrencies.Gems >= price)
                    {
                        //player.VirtualCurrencies.Gems -= price;
                        //item.Count += 1;

                        var filter = MongoDbTools.CreateInventoryFilter(playerId, item.ItemId);
                        var update = MongoDbTools.CreateInventoryUpdate(item.ItemId, 1);
                        update = update.Inc(p => p.VirtualCurrencies.Gems, -price);

                        player = await context.Players.FindAndUpdate(filter,update);

                        //return only modified data
                        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                        {
                            VirtualCurrencies = player.VirtualCurrencies,
                            Inventory = player.Inventory,
                        });
                    }
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


        [HttpPost("SellGood")]
        public async Task<ApiResponse<PlayerData>> SellGood([FromBody]SellGoodRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var playerId = player.Id;
                var good = player.Inventory.AllItems.FirstOrDefault(i => i.ItemId == request.Id);
                if (good?.Count >= request.Count)
                {
                    var price = SharedGlobalData.GetItemPrice(good.ItemId);
                    price = price * request.Count;
                    var finalPrice= (long)Math.Ceiling(price);
                    player.VirtualCurrencies.Gems += finalPrice;
                    
                    good.Count -= request.Count;


                    var filter = MongoDbTools.CreateInventoryFilter(playerId, good.ItemId);
                    var update = MongoDbTools.CreateInventoryUpdate(good.ItemId, -request.Count);
                    update = update.Inc(p => p.VirtualCurrencies.Gems, finalPrice);

                    player = await context.Players.FindAndUpdate(filter, update);


                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        Inventory = player.Inventory,
                    });

                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }
    }
}