using System.Linq;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DieupeGames.Controllers.PacRun
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<LeaderBoardController> logger;

        public StoreController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<LeaderBoardController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        [HttpPost("ConfirmPurchase")]
        public async Task<ApiResponse<PlayerData>> ConfirmPurchase([FromBody]ConfirmPurchaseRequest request)
        {
            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var confirmPurchaseResponse = new ConfirmPurchaseResponse()
                {
                    ProductId = request.ProductId,
                    Success = false,
                };
                var verified = false;
                var receiptData = request.AndroidReceiptData;
                var productId = request.ProductId;
                switch (request.AppStoreEnum)
                {
                    case AppStoreEnum.GooglePlay:
                        //todo verify the receipt
                        verified = true;
                        break;

                    default:
                        verified = true;
                        break;
                }

                if (verified)
                {
                    var shopItem = ShopItemList.ItemList.FirstOrDefault(it => it.StoreCode == productId);
                    var available = player.HasLegendaryStart == false;
                    if (shopItem != null)
                    {
                        var playerId = player.Id;
                        if (shopItem.ShopItemId == ShopItemId.LegendaryStart)
                        {
                            if (available)
                            {
                                //playerData.AddShopItem(shopItem);
                                foreach (var payout in shopItem.PayoutList)
                                {
                                    await context.Players.UpdateInventoryItem(playerId, payout.ItemId, (int)payout.Amount);
                                }

                                player = await context.Players
                                      .FindAndUpdate(player, Builders<PlayerData>.Update.Set(p => p.HasLegendaryStart, true));

                                //playerData.LegendaryStartPromoAvailable = false;
                            }
                        }
                        else
                        {
                            //playerData.AddShopItem(shopItem);
                            foreach (var payout in shopItem.PayoutList)
                            {
                                player = await context.Players.UpdateInventoryItem(playerId, payout.ItemId, (int)payout.Amount);
                            }
                        }

                        //playerData = await context.PlayerData.Update(playerData);

                        confirmPurchaseResponse.Success = true;

                        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                        {
                            HasLegendaryStart = player.HasLegendaryStart,
                            VirtualCurrencies = player.VirtualCurrencies,
                            Inventory = player.Inventory,
                            PurchaseResponse = confirmPurchaseResponse
                        });
                    }


                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


    }
}