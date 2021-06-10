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
    public class SyncAccountController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public SyncAccountController(MongoDBContext context, IOptions<AppSettings> appSettings,
            IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }


        [HttpPost("SyncWithUniversalLogin")]
        public async Task<ApiResponse<PlayerData>> SyncWithUniversalLogin([FromBody]AuthenticateRequest request)
        {

            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            if (request.Username.IsEmptyString() || request.Password.IsEmptyString())
            {
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
            }
            else
            {

                var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);

                if (player != null)
                {
                    var existingPlayer = await context.Players.Find(p => p.UserName == request.Username);
                    if (existingPlayer != null)
                    {
                        return ApiResponse<PlayerData>.CreateError(ApiResponseCode.UserNameTaken);
                    }
                    else
                    {
                        //todo sanitize data

                        var salt = ServerTools.GenerateRandomSalt();
                        var password = ServerTools.HashPassword(request.Password, salt);

                        var update = Builders<PlayerData>.Update
                            .Set(p => p.Profile.UserName, request.Username)
                            .Set(p => p.Profile.Salt, salt)
                            .Set(p => p.Profile.PassWord, password)
                            .Set(p => p.Profile.HasUniversalLogin, true)
                            .Set(p => p.Profile.IsAnonymousAccount, false)

                            .Set(p => p.UserName, request.Username)
                            .Set(p => p.GlobalLeaderBoardEntry.UserName, request.Username);

                        player = await context.Players.FindAndUpdate(player, update);


                        return ApiResponse<PlayerData>.CreateSuccess(player);
                    }
                }
                else
                {
                    return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
                }

            }

        }



    }
}