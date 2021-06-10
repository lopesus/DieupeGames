using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public AuthenticationController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }



        [HttpPost("AuthenticateAnonymous")]
        public async Task<ApiResponse<PlayerData>> AuthenticateAnonymous([FromBody]AuthenticateRequest request)
        {
            if (request != null)
            {
                if (request.DeviceId.IsEmptyString() != false) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData, "device id null or empty");

                var playerData = await context.Players
                    .Find(p => p.DeviceId == request.DeviceId);

                var sessionId = Guid.NewGuid().ToString();

                if (playerData == null)
                {
                    //create new user
                    playerData = new PlayerData()
                    {
                        DeviceId = request.DeviceId,
                        UserName = request.Username,
                        IsTester = request.IsTester,
                        Profile = new PlayerProfile()
                        {
                            IsAnonymousAccount = true,
                        }
                    };
                    playerData.Initialize();
                    await context.Players.Create(playerData);


                    // new user automatically authenticate and  generate jwt token
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    //playerData.Token = token;
                    //playerData.SessionId = sessionId;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);
                }
                else
                {
                    // returning anonymous player
                    if (playerData.Profile?.IsAnonymousAccount == true)
                    {
                        var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                        //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                        var update = Builders<PlayerData>.Update
                            .Set(p => p.LastLogin, DateTime.UtcNow)
                            .Set(p => p.Token, token)
                            .Set(p => p.SessionId, sessionId);

                        playerData = await context.Players.FindAndUpdate(playerData, update);

                        return ApiResponse<PlayerData>.CreateSuccess(playerData);
                    }
                    else
                    {
                        //create a new account to allow th player to enter the game,
                        //should normally login after to reload previuous data
                        //create new user
                        playerData = new PlayerData()
                        {
                            DeviceId = Guid.NewGuid().ToString(),
                            UserName = request.Username,
                            IsTester = request.IsTester,
                            Profile = new PlayerProfile()
                            {
                                IsAnonymousAccount = true,
                            }
                        };
                        playerData.Initialize();
                        await context.Players.Create(playerData);


                        // new user automatically authenticate and  generate jwt token
                        var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                        //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                        //playerData.Token = token;
                        //playerData.SessionId = sessionId;

                        var update = Builders<PlayerData>.Update
                            .Set(p => p.LastLogin, DateTime.UtcNow)
                            .Set(p => p.Token, token)
                            .Set(p => p.SessionId, sessionId);

                        playerData = await context.Players.FindAndUpdate(playerData, update);

                        return ApiResponse<PlayerData>.CreateSuccess(playerData);

                    }

                    return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerAlreadyExist);
                }


            }

            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

        }

        //[Authorize]
        [HttpPost("CreateAccount")]
        public async Task<ApiResponse<PlayerData>> CreateAccount([FromBody]AuthenticateRequest request)
        {
            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            if (request.Username.IsEmptyString() || request.Password.IsEmptyString())
            {
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
            }
            else
            {
                var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
                //var currentPlayer = await JwtTools.GetCurrentPlayer(request.Token,appSettings.Secret, context);

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

                    //playerData.Profile.UserName = request.Username;
                    //playerData.Profile.Salt = salt;
                    //playerData.Profile.PassWord = password;
                    //playerData.Profile.IsAnonymousAccount = false;
                    //playerData.UserName = request.Username;
                    //playerData.GlobalLeaderBoardEntry.UserName = request.Username;

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

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ApiResponse<PlayerData>> Login([FromBody]AuthenticateRequest request)
        {
            //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);

            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            if (request.Username.IsEmptyString() || request.Password.IsEmptyString())
            {
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);
            }
            else
            {
                // var currentPlayer = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);

                var playerData = await context.Players.Find(p => p.UserName == request.Username);
                if (playerData != null)
                {
                    //todo sanitize data
                    var pass = ServerTools.HashPassword(request.Password, playerData.Profile.Salt);
                    if (pass.Equals(playerData.Profile.PassWord))
                    {
                        //  generate jwt token
                        // var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);
                        var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);

                        var sessionId = Guid.NewGuid().ToString();

                        var update = Builders<PlayerData>.Update
                            .Set(p => p.LastLogin, DateTime.UtcNow)
                            .Set(p => p.Token, token)
                            .Set(p => p.SessionId, sessionId);


                        await context.Players.FindAndUpdate(playerData, update);
                        return ApiResponse<PlayerData>.CreateSuccess(playerData);
                    }
                    else
                    {
                        return ApiResponse<PlayerData>.CreateError(ApiResponseCode.BadCredentials);
                    }
                }
                else
                {
                    return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
                }

            }

        }


        [AllowAnonymous]
        [HttpPost("LoginWithFaceBook")]
        public async Task<ApiResponse<PlayerData>> LoginWithFaceBook([FromBody]AuthenticateRequest request)
        {
            if (request != null)
            {
                if (request.FaceBookId.IsEmptyString() != false) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData, "FaceBookId  null or empty");

                var playerData = await context.Players
                    .Find(p => p.FaceBookId == request.FaceBookId);

                var sessionId = Guid.NewGuid().ToString();

                if (playerData == null)
                {
                    //create new user
                    playerData = new PlayerData()
                    {
                        FaceBookId = request.FaceBookId,
                        DeviceId = request.DeviceId,
                        UserName = request.Username.ExtractUsername(),        
                        IsTester = request.IsTester,
                        Profile = new PlayerProfile()
                        {
                            IsAnonymousAccount = false,
                            HasFacebookLogin = true,
                        }
                    };
                    playerData.Initialize();
                    await context.Players.Create(playerData);


                    // new user automatically authenticate and  generate jwt token
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    //playerData.Token = token;
                    //playerData.SessionId = sessionId;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);
                }
                else
                {
                    ////to update player with full name . to remove after
                    //var userName = playerData.UserName.ExtractUsername();
                    //playerData.UserName = userName;


                    // returning facebook  player
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        //to update player with full name . to remove after
                        //.Set(p => p.UserName, userName)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);

                    //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerAlreadyExist);
                }


            }

            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

        }


        [AllowAnonymous]
        [HttpPost("LoginWithKartRidge")]
        public async Task<ApiResponse<PlayerData>> LoginWithKartRidge([FromBody]AuthenticateRequest request)
        {
            if (request != null)
            {
                if (request.KartRidgeId.IsEmptyString() != false) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData, "KartRidgeId  null or empty");

                var playerData = await context.Players
                    .Find(p => p.KartRidgeId == request.KartRidgeId);

                var sessionId = Guid.NewGuid().ToString();

                if (playerData == null)
                {
                    //create new user
                    playerData = new PlayerData()
                    {
                        KartRidgeId = request.KartRidgeId,
                        DeviceId = request.DeviceId,
                        UserName = request.Username,
                        Profile = new PlayerProfile()
                        {
                            IsAnonymousAccount = false,
                            HasKartRidgeLogin = true,
                        }
                    };
                    playerData.Initialize();
                    await context.Players.Create(playerData);


                    // new user automatically authenticate and  generate jwt token
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    //playerData.Token = token;
                    //playerData.SessionId = sessionId;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);
                }
                else
                {
                    // returning KartRidge  player
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        //to update player with full name . to remove after
                        //.Set(p => p.UserName, userName)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);

                    //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerAlreadyExist);
                }


            }

            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

        }

        [AllowAnonymous]
        [HttpPost("LoginWithSteam")]
        public async Task<ApiResponse<PlayerData>> LoginWithSteam([FromBody]AuthenticateRequest request)
        {
            if (request != null)
            {
                if (request.SteamId.IsEmptyString() != false) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData, "SteamId  null or empty");

                var playerData = await context.Players
                    .Find(p => p.SteamId == request.SteamId);

                var sessionId = Guid.NewGuid().ToString();

                if (playerData == null)
                {
                    //create new user
                    playerData = new PlayerData()
                    {
                        SteamId = request.SteamId,
                        DeviceId = request.DeviceId,
                        UserName = request.Username,
                        Profile = new PlayerProfile()
                        {
                            IsAnonymousAccount = false,
                            HasSteamLogin = true,
                        }
                    };
                    playerData.Initialize();
                    await context.Players.Create(playerData);


                    // new user automatically authenticate and  generate jwt token
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    //playerData.Token = token;
                    //playerData.SessionId = sessionId;

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);
                }
                else
                {
                    ////to update player with full name . to remove after
                    //var userName = playerData.UserName;
                    //playerData.UserName = userName;


                    // returning facebook  player
                    var token = JwtTools.CreateJwtToken(playerData, appSettings.Secret);
                    //var token = EncryptionTools.EncryptString(playerData.Id, appSettings.Secret);

                    var update = Builders<PlayerData>.Update
                        .Set(p => p.LastLogin, DateTime.UtcNow)
                        .Set(p => p.Token, token)
                        //to update player with full name . to remove after
                        //.Set(p => p.UserName, userName)
                        .Set(p => p.SessionId, sessionId);

                    playerData = await context.Players.FindAndUpdate(playerData, update);

                    return ApiResponse<PlayerData>.CreateSuccess(playerData);

                    //return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerAlreadyExist);
                }


            }

            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

        }



       

    }
}