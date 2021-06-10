using System.Security.Claims;
using System.Threading.Tasks;
using DieupeGames.Data;
using DieupeGames.Data.Mongo;
using DieupeGames.Helpers;
using DieupeGames.Services;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Request;
using LabirunModel.Labirun.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.Annotations;

namespace DieupeGames.Controllers.PacRun
{
    [OpenApiIgnore]
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public TestApiController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        // GET: player/generate/count
        [HttpGet]
        [Route("generate/{count:int}")]
        public async Task<long> GetGeneratePlayer(int count)
        {
            DbInitializer.GenerateMongoPlayer(context, count);
            var res = await context.Players.Count(p => true);
            return res;
        }

        [HttpPost("PostAction")]
        public async Task<ApiResponse<PlayerData>> PostAction([FromBody]AuthenticateRequest request)
        {

             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {
                if (true)
                {

                    //playerData = await context.Players.Update(playerData);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = playerData.VirtualCurrencies,
                        Bank = playerData.Bank,
                    });
                }
                //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }
    }
}