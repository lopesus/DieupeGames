using System;
using System.Linq;
using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Services;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Request;
using LabirunModel.Labirun.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DieupeGames.Controllers.PacRun
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderBoardController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<LeaderBoardController> logger;
        private readonly LeaderBoardService LeaderBoardService;

        public LeaderBoardController(MongoDBContext context, IOptions<AppSettings> appSettings, LeaderBoardService leaderBoardCacheClient, IDistributedCache cache, ILogger<LeaderBoardController> logger)
        {
            this.LeaderBoardService = leaderBoardCacheClient;
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }


        [HttpPost("GetRankAroundFromCache")]
        public async Task<ApiResponse<PlayerData>> GetRankAroundFromCache([FromBody] LeaderBoardRequest request)
        {
            try
            {
                var result = await LeaderBoardService.GetRankAround(request);

                return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                {
                    ServerResponse = new ServerResponse()
                    {
                        LeaderBoardResult = result
                    }
                });
            }
            catch (Exception e)
            {
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
        }


        [HttpPost("InitializeGlobalLeaderBoardCache")]
        public async Task<BasicResponse> InitializeGlobalLeaderBoardCache()
        {
            try
            {
                var temp = await context.Players.GetAll();
                var entries = temp.Select(p => p.GlobalLeaderBoardEntry).ToList();

                var leaderBoardRequest = new LeaderBoardRequest("")
                {
                    InitialEntryList = entries,
                };

                //no await is correct here, fire and forget 
                logger.LogInformation($"initializing leader with{entries.Count} entries ");
                LeaderBoardService.Initialize(leaderBoardRequest);

                return new BasicResponse();


            }
            catch (Exception e)
            {
                return new BasicResponse(); 
            }

        }

        //[HttpPost("GetLeaderBoard")]
        //public async Task<ApiResponse<PlayerData>> GetLeaderBoard([FromBody]LeaderBoardRequest request)
        //{

        //    // var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
        //    var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

        //    if (playerData != null)
        //    {
        //        var cachedData = "";// await cache.GetStringAsync(request.LeaderBoardId.ToString());
        //        List<GlobalLeaderBoardEntry> leaderBoardEntries = new List<GlobalLeaderBoardEntry>();
        //        if (cachedData.IsEmptyString())
        //        {
        //            //get data from db 
        //            var temp = await context.LeaderBoard.GetAll();
        //            IOrderedEnumerable<GlobalLeaderBoardEntry> boardEntries = null;
        //            switch (request.LeaderBoardId)
        //            {

        //                case LeaderBoardId.TotalPoint:
        //                    boardEntries = temp.OrderByDescending(e => e.TotalPoint);
        //                    break;
        //                case LeaderBoardId.Combo4Ghost:
        //                    boardEntries = temp.OrderByDescending(e => e.TotalPoint);
        //                    break;
        //                case LeaderBoardId.SuperCombo4Ghost:
        //                    boardEntries = temp.OrderByDescending(e => e.TotalPoint);
        //                    break;
        //            }

        //            if (boardEntries != null)
        //            {
        //                leaderBoardEntries = boardEntries.ToList();
        //                var json = leaderBoardEntries.ToJsonDotNetCore();

        //                //save a copy in the cache 
        //                await cache.SetStringAsync(request.LeaderBoardId.ToString(), json);
        //            }
        //        }
        //        else
        //        {
        //            leaderBoardEntries = cachedData.FromJson<List<GlobalLeaderBoardEntry>>();
        //        }


        //        LeaderBoardResult result = new LeaderBoardResult();
        //        result.LeaderBoardId = request.LeaderBoardId;

        //        var f10 = leaderBoardEntries.Take(10).ToList();
        //        result.First10 = f10;


        //        var entry = leaderBoardEntries.FirstOrDefault(i => i.Id == playerData.Id);

        //        if (entry != null)
        //        {
        //            var rank = leaderBoardEntries.IndexOf(entry);
        //            entry.Rank = rank;
        //            result.PlayerEntry = entry;

        //            //take 3 before
        //            for (int i = 3; i > 0; i--)
        //            {
        //                var index = rank - i;
        //                if (index >= 0 && leaderBoardEntries.Count > index)
        //                {
        //                    var boardEntry = leaderBoardEntries[index];
        //                    boardEntry.Rank = index;
        //                    result.BeforePlayer.Add(boardEntry);
        //                }
        //            }

        //            //take 3 after 
        //            for (int i = 1; i <= 3; i++)
        //            {
        //                var index = rank + i;
        //                if (index >= 0 && leaderBoardEntries.Count > index)
        //                {
        //                    var boardEntry = leaderBoardEntries[index];
        //                    boardEntry.Rank = index;
        //                    result.AfterPlayer.Add(boardEntry);
        //                }
        //            }
        //        }



        //        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
        //        {
        //            ServerResponse = new ServerResponse()
        //            {
        //                LeaderBoardResult = result
        //            }
        //        });

        //        //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
        //    }
        //    return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        //}


        //[HttpPost("PostScore")]
        //public async Task<ApiResponse<GlobalLeaderBoardEntry>> PostScore([FromBody]LeaderBoardRequest request)
        //{
        //    // var id = JwtTools.GetPlayerIdFromClaims(HttpContext.User.Identity as ClaimsIdentity);

        //    // var playerData = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
        //    var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

        //    if (playerData != null)
        //    {
        //        var entry = await context.LeaderBoard.Find(l => l.Id == playerData.Id);
        //        if (entry == null)
        //        {
        //            entry = new GlobalLeaderBoardEntry()
        //            {
        //                Id = playerData.Id,
        //                UserName = playerData.UserName,
        //                UpDateTime = DateTime.UtcNow,
        //            };
        //            entry.AddToScore(request);
        //            await context.LeaderBoard.Create(entry);
        //        }
        //        else
        //        {
        //            entry.UpDateTime = DateTime.UtcNow;
        //            entry.AddToScore(request);
        //            entry = await context.LeaderBoard.Update(entry);

        //        }
        //        return ApiResponse<GlobalLeaderBoardEntry>.CreateSuccess(entry);

        //        //return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.Error);
        //    }
        //    return ApiResponse<GlobalLeaderBoardEntry>.CreateError(ApiResponseCode.PlayerNotFound);
        //}
    }

  //[Authorize]
}