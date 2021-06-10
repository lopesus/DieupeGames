using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LabirunModel.Config;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;
using LabirunModel.Labirun.Request;
using LabirunModel.Labirun.Response;
using LabirunModel.Tools;
using LabirunServer.Helpers;
using LabirunServer.Services;
using learnCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LabirunServer.Controllers.Labirun
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FactoryController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public FactoryController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }



        [HttpPost("StartProduction")]
        public async Task<ApiResponse<PlayerData>> StartProduction([FromBody]StartProductionRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var goodFactory = player.GoodFactory;
                if (goodFactory?.Status == ProductionStatus.Idle)
                {

                    goodFactory.Status = ProductionStatus.InProduction;
                    goodFactory.StartTime = DateTime.UtcNow;
                    goodFactory.ProductionTime = request.Time;

                    var time = SharedGlobalData.GetProductionTimeInHour(request.Time);



#if DEBUG
                    //for test
                    goodFactory.EndTime = goodFactory.StartTime.AddSeconds(10);
#else
                    goodFactory.EndTime = goodFactory.StartTime.AddHours(time);
#endif

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.GoodFactory, player.GoodFactory);
                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        GoodFactory = player.GoodFactory,
                    });
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

        [HttpPost("ApplyBoost")]
        public async Task<ApiResponse<PlayerData>> ApplyBoost([FromBody]ApplyFactoryBoostRequest request)
        {
            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var invBoost = player.Inventory.AllItems.FirstOrDefault(i => i.ItemId == request.BoostId);
                var factoryBoost = player.GoodFactory.Boost.FirstOrDefault(b => b.Slot == request.Slot);
                if (invBoost != null && invBoost.Count > 0 && factoryBoost != null)
                {
                    factoryBoost.ItemId = request.BoostId;
                    switch (request.Slot)
                    {
                        case BoostSlot.Daily1:
                        case BoostSlot.Daily2:
                        case BoostSlot.Daily3:
                        case BoostSlot.Daily4:
#if DEBUG
                            factoryBoost.EndTime = DateTime.UtcNow.AddSeconds(20);
#else 
                            factoryBoost.EndTime = DateTime.UtcNow.AddDays(1);
#endif
                            break;
                        case BoostSlot.Monthly1:
                        case BoostSlot.Monthly2:
                        case BoostSlot.Monthly3:
                        case BoostSlot.Monthly200:
                            factoryBoost.EndTime = DateTime.UtcNow.AddDays(30);
                            break;
                        default:
                            break;
                    }

                    factoryBoost.Value = SharedGlobalData.GetBoostValue(factoryBoost.ItemId);
                    // consume the invBoost, == remove it in the inventory
                    invBoost.Count -= 1;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.Inventory, player.Inventory)
                        .Set(p => p.GoodFactory.Boost, player.GoodFactory.Boost);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        GoodFactory = player.GoodFactory,
                        Inventory = player.Inventory,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

        [HttpPost("StopProduction")]
        public async Task<ApiResponse<PlayerData>> StopProduction(BasicRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var goodFactory = player.GoodFactory;
                if (goodFactory != null)
                {
                    goodFactory.Status = ProductionStatus.Idle;
                    var update = Builders<PlayerData>.Update
                        .Set(p => p.GoodFactory, goodFactory);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        GoodFactory = player.GoodFactory,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost("CollectProduction")]
        public async Task<ApiResponse<PlayerData>> CollectProduction(BasicRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var goodFactory = player.GoodFactory;
                if (goodFactory != null)
                {
                    if (goodFactory.EndTime.IsExpired())
                    {
                        var ratio = SharedGlobalData.GoodFactoryProductionRatio
                            .FirstOrDefault(p => p.Key == goodFactory.ProductionTime);

                        var hourRatio = ratio.Val / 100;

                        // calculate factory boosts sum
                        var boostSum = 0;

                        foreach (FactoryBoost boost in goodFactory.Boost)
                        {
                            if (boost.EndTime.IsActive())
                            {
                                boostSum += boost.Value;
                            }
                        }

                        foreach (var productionLine in goodFactory.ProductionLines)
                        {
                            if (productionLine.State == ProductionLineState.Open)
                            {
                                var product = productionLine.Product;
                                double production = product.ProdPerHour;
                                //apply the productionline bonus percentage
                                production += production * productionLine.ProdBonus / 100;

                                //adjust for the production time
                                production = production * hourRatio;

                                //apply factory bonus percent
                                production += production * (boostSum + goodFactory.ProductionBonus) / 100;

                                //round the result
                                production = Math.Ceiling(production);

                                //update count in inventory
                                var item = player.Inventory.AllItems.FirstOrDefault(i => i.ItemId == product.Id);
                                if (item != null) item.Count += (int)production;
                            }
                        }

                        //goodFactory.Status = ProductionStatus.Idle;
                        var update = Builders<PlayerData>.Update.Set(p => p.GoodFactory.Status, ProductionStatus.Idle)
                            .Set(p => p.Inventory, player.Inventory);
                        player = await context.Players.FindAndUpdate(player, update);

                        //return only modified data
                        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                        {
                            Inventory = player.Inventory,
                            GoodFactory = player.GoodFactory,
                        });

                    }

                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


        [HttpPost("UpgradeGoodFactory")]
        public async Task<ApiResponse<PlayerData>> UpgradeGoodFactory(BasicRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //  var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var goodFactory = player.GoodFactory;
                if (goodFactory != null)
                {
                    UpgradeItem upgradeItem = SharedGlobalData.FactoryUpgrade.FirstOrDefault(u => u.Key == goodFactory.UpgradeCount + 1);
                    if (upgradeItem != null && player.VirtualCurrencies.Gems >= upgradeItem.Cost)
                    {
                        player.VirtualCurrencies.Gems -= upgradeItem.Cost;
                        goodFactory.UpgradeCount += 1;
                        goodFactory.ProductionBonus += (int)upgradeItem.Increment;

                    }
                    var update = Builders<PlayerData>.Update
                        .Inc(p => p.VirtualCurrencies.Gems, -upgradeItem.Cost)
                        .Inc(p => p.GoodFactory.UpgradeCount, 1)
                        .Inc(p => p.GoodFactory.ProductionBonus, upgradeItem.Increment);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        GoodFactory = player.GoodFactory,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


        [HttpPost("UpgradeProductionLine")]
        public async Task<ApiResponse<PlayerData>> UpgradeProductionLine([FromBody] UpgradeItemRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var goodFactory = player.GoodFactory;
                if (goodFactory != null)
                {
                    var productionLine = goodFactory.ProductionLines.FirstOrDefault(l => l.Id == (int)request.Id);
                    var upgradeItem = SharedGlobalData.ProductionLineUpgrade.FirstOrDefault(l => productionLine != null && l.Key == productionLine.UpgradeCount + 1);

                    if (productionLine != null && upgradeItem != null)
                    {
                        if (player.VirtualCurrencies.Gems >= upgradeItem.Cost)
                        {
                            player.VirtualCurrencies.Gems -= upgradeItem.Cost;
                            productionLine.UpgradeCount += 1;
                            productionLine.ProdBonus += (int)upgradeItem.Increment;

                            var update = Builders<PlayerData>.Update
                                .Inc(p => p.VirtualCurrencies.Gems, -upgradeItem.Cost)
                                .Set(p => p.GoodFactory.ProductionLines, player.GoodFactory.ProductionLines);

                            player = await context.Players.FindAndUpdate(player, update);

                            //return only modified data
                            return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                            {
                                VirtualCurrencies = player.VirtualCurrencies,
                                GoodFactory = player.GoodFactory,
                            });
                        }
                    }
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost("UpgradeProductProductionPerHour")]
        public async Task<ApiResponse<PlayerData>> UpgradeProductProductionPerHour([FromBody] UpgradeItemRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player?.GoodFactory != null)
            {
                var goodFactory = player.GoodFactory;

                var productionLine = goodFactory.ProductionLines.FirstOrDefault(l => l.Id == (int)request.Id);
                var product = productionLine.Product;
                var upgradeItem = SharedGlobalData.ProductProductionPerHourUpgrade.FirstOrDefault(l => l.Key == product.UpgradeCount + 1);

                if (player.VirtualCurrencies.Gems >= upgradeItem.Cost)
                {
                    player.VirtualCurrencies.Gems -= upgradeItem.Cost;
                    product.UpgradeCount += 1;
                    product.ProdPerHour += (int)upgradeItem.Increment;

                    var update = Builders<PlayerData>.Update
                        .Inc(p => p.VirtualCurrencies.Gems, -upgradeItem.Cost)
                        .Set(p => p.GoodFactory.ProductionLines, player.GoodFactory.ProductionLines);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        GoodFactory = player.GoodFactory,
                    });
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


    }
}