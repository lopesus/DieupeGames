//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using LabirunModel.Config;
//using LabirunModel.Labirun;
//using LabirunModel.Labirun.Enums;
//using LabirunModel.Labirun.Request;
//using LabirunModel.Labirun.Response;
//using LabirunServer.Helpers;
//using LabirunServer.Services;
//using learnCore;
//using learnCore.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;
//using NSwag.Annotations;

//namespace LabirunServer.Controllers.Labirun
//{
//    [Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PublicMazeController : ControllerBase
//    {
//        private readonly MongoDBContext context;
//        private readonly AppSettings appSettings;
//        private IDistributedCache cache;
//        private ILogger<PlayerController> logger;
//        private readonly ICreatedMazeCacheClient service;

//        public PublicMazeController(MongoDBContext context, IOptions<AppSettings> appSettings, ICreatedMazeCacheClient service, IDistributedCache cache, ILogger<PlayerController> logger)
//        {
//            this.service = service;
//            this.context = context;
//            this.appSettings = appSettings.Value;
//            this.cache = cache;
//            this.logger = logger;
//        }

//        [HttpPost("GetAllMaze")]
//        public async Task<ApiResponse<PublicMazeResponse>> GetAllMaze([FromBody]RangeRequest request)
//        {
//            if (request == null)
//            {
//                request = new RangeRequest(1, 20);
//            }

//            if (request.ItemsPerPage == 0)
//            {
//                request.ItemsPerPage = 10;
//            }

//            long maxPage = 1;
//            var counter = await context.Counters.GetCounter();
//            if (counter != null)
//            {
//                maxPage = counter.CreatedMazeCounter / request.ItemsPerPage;

//                if (counter.CreatedMazeCounter % request.ItemsPerPage != 0)
//                {
//                    maxPage += 1;
//                }

//                if (request.PageNumber > maxPage)
//                {
//                    request.PageNumber = maxPage;
//                }
//            }

//            var temp = await context.PublicMazes.GetRange(request);
//            var mazes = temp.ToList();
//            var response = new PublicMazeResponse(mazes)
//            {
//                MaxPage = (int)maxPage,
//            };
//            if (counter != null) response.TotalMazeCount = counter.CreatedMazeCounter;
//            return ApiResponse<PublicMazeResponse>.CreateSuccess(response);
//        }



//        [HttpPost("GetMaze")]
//        public async Task<ApiResponse<CreatedMaze>> GetMaze([FromBody]CreatedMaze maze)
//        {


//            var existingMaze = await context.PublicMazes.Find(m => m.Id == maze.Id);
//            if (existingMaze != null)
//            {
//                //return only modified data
//                return ApiResponse<CreatedMaze>.CreateSuccess(existingMaze);
//            }
//            else
//            {
//                return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.NotFound);
//            }
//        }



//        [HttpPost("CreateMaze")]
//        public async Task<ApiResponse<PlayerData>> CreateMaze([FromBody]CreatedMaze maze)
//        {

//            if (maze?.Id == null)
//            {
//                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
//            }

//            //var playerData = await JwtTools.GetCurrentPlayer(maze.PlayerId, appSettings.Secret, context);
//            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);

//            if (player == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);

            
//            //check 
//            var exist = player.PlayerMazes.GetMaze(maze.Id);
//            if (exist == null)
//            {
//                // check limit 
//                if (player.PlayerMazes.PlayerMazes.Count >= SharedGlobalData.MaxCreatedMazePerPlayer)
//                {
//                    return ApiResponse<PlayerData>.CreateError(ApiResponseCode.MaxCreatedMazePerPlayerReached);
//                }

//                // insert a new maze
//                maze.CreatorName = player.UserName;
//                player.PlayerMazes.AddMaze(maze);

//                var update = Builders<PlayerData>.Update.Set(p => p.PlayerMazes, player.PlayerMazes);
//                player = await context.Players.FindAndUpdate(player, update);
//            }
//            else
//            {
//                // save to player
//                player.PlayerMazes.UpdateMaze(maze);
//                var update = Builders<PlayerData>.Update.Set(p => p.PlayerMazes, player.PlayerMazes);
//                player = await context.Players.FindAndUpdate(player, update);
//            }

//            //return only modified data
//            return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
//            {
//                Id = player.Id,
//                PlayerMazes = player.PlayerMazes,
//            });
//            //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
//        }

//        [HttpPost("DeleteMaze")]
//        public async Task<ApiResponse<PlayerData>> DeleteMaze([FromBody]CreatedMaze maze)
//        {

//            if (maze?.Id == null)
//            {
//                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
//            }
//            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
//            // var playerData = await JwtTools.GetCurrentPlayer(maze.PlayerId, appSettings.Secret, context);

//            if (player != null)
//            {
//                {
//                    player.PlayerMazes.DeleteMaze(maze);
//                    var update = Builders<PlayerData>.Update.Set(p => p.PlayerMazes, player.PlayerMazes);
//                    player = await context.Players.FindAndUpdate(player, update);

//                    //return only modified data
//                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
//                    {
//                        PlayerMazes = player.PlayerMazes,
//                    });
//                }
//                //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
//            }
//            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
//        }

//        [HttpPost("MakeMazePublic")]
//        public async Task<ApiResponse<PlayerData>> MakeMazePublic([FromBody]CreatedMaze maze)
//        {

//            if (maze?.Id == null)
//            {
//                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
//            }
//            // var playerData = await JwtTools.GetCurrentPlayer(maze.PlayerId, appSettings.Secret, context);
//            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);

//            if (player != null)
//            {
//                CreatedMaze existingMaze = player.PlayerMazes.GetMaze(maze.Id);

//                if (existingMaze != null)
//                {
//                    if (existingMaze.IsPublic == false)
//                    {
//                        existingMaze.IsPublic = true;

//                        var serverCounter = await context.Counters.GetMazeCounter();
//                        if (serverCounter != null) existingMaze.SystemName = serverCounter.CreatedMazeCounter;

                       
//                       await context.PublicMazes.Create(existingMaze);


//                        var update = Builders<PlayerData>.Update.Set(p => p.PlayerMazes, player.PlayerMazes);
//                        player = await context.Players.FindAndUpdate(player, update);


//                        //return only modified data
//                        return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
//                        {
//                            Id = player.Id,
//                            PlayerMazes = player.PlayerMazes,
//                        });
//                    }

//                }

//                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
//            }
//            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
//        }


//        [HttpPost("PostScore")]
//        public async Task<ApiResponse<CreatedMaze>> PostScore([FromBody]CreatedMazeRequest request)
//        {

//            if (request?.Id == null)
//            {
//                return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.EmptyRequestData);
//            }
//            // var playerData = await JwtTools.GetCurrentPlayer(maze.PlayerId, appSettings.Secret, context);
//            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);

//            if (player == null) return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.PlayerNotFound);
//            //if (player.UserName == null) return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.UserNameNotSet);

//            CreatedMaze maze = await context.PublicMazes.Find(m => m.Id == request.Id);

//            if (maze != null)
//            {
//                //set best score and player
//                var update = Builders<CreatedMaze>.Update.Inc(p => p.PlayCount, 1);
//                if (request.PlayStyle == MazePlayStyle.BestScore && request.BestScore > maze.BestScore)
//                {
//                    update = update.Set(m => m.BestScore, request.BestScore)
//                        .Set(m => m.BestDate, DateTime.UtcNow)
//                        .Set(m => m.BestPlayer, player.UserName);
//                }

//                //set fastest score and player
//                if (request.PlayStyle == MazePlayStyle.Fastest && request.FastestScore > 0 && (request.FastestScore <= maze.FastestScore || maze.FastestScore == 0))
//                {
//                    update = update.Set(m => m.FastestScore, request.FastestScore)
//                        .Set(m => m.FastDate, DateTime.UtcNow)
//                        .Set(m => m.FastestPlayer, player.UserName);
//                }

//                maze = await context.PublicMazes.FindAndUpdate(maze, update);


//                //return only modified data
//                return ApiResponse<CreatedMaze>.CreateSuccess(maze);

//            }

//            return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.Error);
//        }



//        [HttpPost("PublishSystemMaze")]
//        public async Task<ApiResponse<CreatedMaze>> PublishSystemMaze([FromBody]CreatedMaze maze)
//        {
//            if (maze == null) return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.Error);

//            var count = await context.Counters.Count();
//            if (count == 0)
//            {
//                await context.Counters.Initialise();
//            }

//            // max 20 system maze
//            if (maze.SystemName > 20)
//            {
//                return ApiResponse<CreatedMaze>.CreateError(ApiResponseCode.Error);
//            }

//            var exist = await context.PublicMazes.Find(m => m.SystemName == maze.SystemName);
//            if (exist == null)
//            {
//                maze.CreatorName = "labirun";
//                maze.IsPublic = true;

//                //var serverCounter = await context.Counters.GetMazeCounter();
//                //if (serverCounter != null) maze.SystemName = serverCounter.CreatedMazeCounter;

//                await context.PublicMazes.Create(maze);
//            }
//            else
//            {
//                var update = Builders<CreatedMaze>.Update
//                    .Set(m => m.Data, maze.Data)
//                    .Set(m => m.Image, maze.Image);
//                maze = await context.PublicMazes.FindAndUpdate(exist, update);
//            }



//            //return only modified data
//            return ApiResponse<CreatedMaze>.CreateSuccess(maze);

//        }



//        //[HttpPost("PublishSystemMazeList")]
//        //public async Task<ApiResponse<PlayerData>> PublishSystemMazeList([FromBody]List<CreatedMaze> mazeList)
//        //{
//        //    foreach (var maze in mazeList)
//        //    {
//        //        maze.CreatorName = "labirun";
//        //        maze.IsPublic = true;
//        //        var serverCounter = await context.Counters.GetMazeCounter();
//        //        if (serverCounter != null)
//        //        {
//        //            maze.SystemName = serverCounter.CreatedMazeCounter;
//        //            maze.Name = serverCounter.CreatedMazeCounter.ToString();
//        //        }
//        //        await context.PublicMazes.Create(maze);
//        //    }

//        //    //return only modified data
//        //    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
//        //    {

//        //    });

//        //}


//    }
//}