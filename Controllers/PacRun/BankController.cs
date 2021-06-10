using System;
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
    public class BankController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public BankController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        [HttpPost]
        [Route("CollectInterest")]
        public async Task<ApiResponse<PlayerData>> CollectInterest(BasicRequest request)
        {
            var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {
                var bank = playerData.Bank;
                if (bank?.IsActive == true)
                {
                    var diff = DateTime.UtcNow - bank.StartTime;
                    var elapsedHour = diff.TotalHours;
                    if (elapsedHour > SharedGlobalData.BankPayoutEvery)
                    {
                        elapsedHour = SharedGlobalData.BankPayoutEvery;
                    }

                    var bankDailyPayout = bank.DailyPayout * elapsedHour / 24;
                    var payout = (int)Math.Ceiling(bankDailyPayout);

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.Bank.StartTime, DateTime.UtcNow)
                        .Inc(p => p.VirtualCurrencies.Gems, payout);

                    //playerData.VirtualCurrencies.Gems += payout;
                    //playerData.Bank.StartTime = DateTime.UtcNow;

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = playerData.VirtualCurrencies,
                        Bank = playerData.Bank,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost]
        [Route("CollectInterestWithBonus")]
        public async Task<ApiResponse<PlayerData>> CollectInterestWithBonus(BasicRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var bank = player.Bank;
                if (bank?.IsActive == true)
                {
                    var diff = DateTime.UtcNow - bank.StartTime;
                    var elapsedHour = diff.TotalHours;
                    if (elapsedHour > SharedGlobalData.BankPayoutEvery)
                    {
                        elapsedHour = SharedGlobalData.BankPayoutEvery;
                    }

                    var bankDailyPayout = bank.DailyPayout * elapsedHour / 24;
                    //apply bonus  of 50%
                    bankDailyPayout += bankDailyPayout * 0.5;
                    var payout = (int)Math.Ceiling(bankDailyPayout);

                    //playerData.VirtualCurrencies.Gems += payout;
                    //playerData.Bank.StartTime = DateTime.UtcNow;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.Bank.StartTime, DateTime.UtcNow)
                        .Inc(p => p.VirtualCurrencies.Gems, payout);


                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        Bank = player.Bank,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }


        [HttpPost]
        [Route("SetDeposit")]
        public async Task<ApiResponse<PlayerData>> SetDeposit(BasicRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var bank = player.Bank;
                if (bank?.IsActive == false && player.VirtualCurrencies.Gems >= SharedGlobalData.BankDeposit)
                {

                    //player.VirtualCurrencies.Gems -= SharedGlobalData.BankDeposit;
                    //player.Bank.StartTime = DateTime.UtcNow;
                    //player.Bank.IsActive = true;

                    var update = Builders<PlayerData>.Update
                        .Inc(p => p.VirtualCurrencies.Gems, -SharedGlobalData.BankDeposit)
                        .Set(p => p.Bank.StartTime, DateTime.UtcNow)
                        .Set(p => p.Bank.IsActive, true);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        Bank = player.Bank,
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }
    }
}