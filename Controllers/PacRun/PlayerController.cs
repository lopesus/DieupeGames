using System.Security.Claims;
using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Helpers;
using DieupeGames.Services;
using LabirunModel.Labirun;
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
    public class PlayerController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public PlayerController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        [HttpPost]
        [Route("GetPlayerData")] // GET: api/player/playerdata/abc
        public async Task<ApiResponse<PlayerData>> GetPlayerData([FromBody]BasicRequest request)
        {
            var headers = HttpContext.Request.Headers;
            var authHeader = headers["Authorization"].ToString();

            var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
                return ApiResponse<PlayerData>.CreateSuccess(playerData);
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
        }



        [HttpPost("SetFriendPromoCode")]
        public async Task<ApiResponse<PlayerData>> SetFriendPromoCode([FromBody]SetFriendPromoCodeRequest request)
        {

             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {
                if (playerData.PromoPlayerId.IsEmptyString() && request.PlayerId.IsNotEmptyString())
                {
                    if (playerData.Id == request.PlayerId)
                    {
                        return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerAndPromoIdAreSame);
                    }
                    else
                    {
                        var promoPlayer = await context.Players.Find(p => p.Id == request.PlayerId);

                        if (promoPlayer == null)
                        {
                            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
                        }

                        //todo use transactions 


                        //update promo count
                        var promoFilter = Builders<PlayerData>.Filter.Eq(p => p.Id, promoPlayer.Id);
                        var promoUpdate = Builders<PlayerData>.Update
                            .Inc(p => p.GameStatistics.PromoCount, 1);

                        ////increase daily payout if necessary
                        //if (promoPlayer.GameStatistics.PromoCount <= GlobalData.MaxPromoBankDailyPayoutReward)
                        //{
                        //    promoUpdate.Inc(p => p.Bank.DailyPayout, GlobalData.PromoBankDailyPayout);
                        //}

                        //save change
                        await context.Players.Increment(promoFilter, promoUpdate);

                        //set promo id
                        playerData.PromoPlayerId = request.PlayerId;

                        var update = Builders<PlayerData>.Update.Set(p => p.PromoPlayerId, request.PlayerId);

                        //save change
                        playerData = await context.Players.FindAndUpdate(playerData,update);

                        //return only modified data
                        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                        {
                            PromoPlayerId = playerData.PromoPlayerId,
                        });
                    }
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

        [HttpPost("Initialize")]
        public async Task<ApiResponse<PlayerData>> Initialize(BasicRequest request)
        {
             var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                player.Initialize();
                UpdateDefinition<PlayerData> updateDefinition = new ObjectUpdateDefinition<PlayerData>(player);
                player = await context.Players.FindAndUpdate(player,updateDefinition);
                return ApiResponse<PlayerData>.CreateSuccess(player);
            }


            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);

        }

        //[HttpGet]
        //[Route("playerdata/{id:alpha}")] // GET: api/player/playerdata/abc
        //public async Task<PlayerData> GetPlayerData(string id)
        //{
        //    var res = await context.PlayerData.FirstOrDefaultAsync(p => p.Id == id);
        //    return res;
        //}
    }
}