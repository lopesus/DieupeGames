using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LabirunModel.Labirun;
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

namespace LabirunServer.Controllers.Labirun
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<LeaderBoardController> logger;

        public TournamentController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<LeaderBoardController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        [HttpPost("GetLeaderBoard")]
        public async Task<ApiResponse<PlayerData>> GetLeaderBoard([FromBody]JoinTournamentRequest request)
        {
             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
        [HttpPost("CollectReward")]
        public async Task<ApiResponse<PlayerData>> CollectReward([FromBody]JoinTournamentRequest request)
        {
            var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
          //  var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
        [HttpPost("Joint")]
        public async Task<ApiResponse<PlayerData>> Joint([FromBody]JoinTournamentRequest request)
        {
             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
        [HttpPost("PostScore")]
        public async Task<ApiResponse<PlayerData>> PostScore([FromBody]JoinTournamentRequest request)
        {
            var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
        [HttpPost("PostTournamentScoreWithResults")]
        public async Task<ApiResponse<PlayerData>> PostTournamentScoreWithResults([FromBody]JoinTournamentRequest request)
        {
             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
        [HttpPost("UpdateStatusAndRank")]
        public async Task<ApiResponse<PlayerData>> UpdateStatusAndRank([FromBody]JoinTournamentRequest request)
        {
             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
       
        [HttpPost("ViewCurrentReward")]
        public async Task<ApiResponse<PlayerData>> ViewCurrentReward([FromBody]JoinTournamentRequest request)
        {
             var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
           // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (playerData != null)
            {

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {

                });

                //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

       
    }
}