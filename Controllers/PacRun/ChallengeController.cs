using System;
using System.Collections.Generic;
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
    public class ChallengeController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;
        private readonly LeaderBoardService leaderBoardCacheClient;

        public ChallengeController(MongoDBContext context, IOptions<AppSettings> appSettings, LeaderBoardService leaderBoardCacheClient, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.leaderBoardCacheClient = leaderBoardCacheClient;
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }



        [HttpPost("UpdateChallengeProgress")]
        public async Task<ApiResponse<PlayerData>> UpdateChallengeProgress([FromBody]ChallengeProgressRequest request)
        {
            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //  var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);
            // todo prevent 2 same player to post from different device simultaneously == cheat

            if (request.LevelFruit > 3)
            {
                request.LevelFruit = 0;//cheater
            }

            if (player != null)
            {
                var playerId = player.Id;
                var maze = player.ChallengeMazeList.FirstOrDefault(m => m.MazeId == request.MazeId);
                if (maze != null && maze.Level == request.MazeLevel /* maze?.IsOpen == true*/)
                {

                    var filter = Builders<PlayerData>.Filter.Where(p => p.Id == playerId);



                    List<GameNotifications> notifications = new List<GameNotifications>();

                    // set next playable level
                    maze.Level += 1;

                    // set next playable level
                    var update = Builders<PlayerData>.Update
                        .Set(p => p.ChallengeMazeList, player.ChallengeMazeList);

                    var gemsGen = new RewardGenerator(SharedGlobalData.AllSeeds.ChallengeGemsMazeSeed);
                    var promoRewardGen = new RewardGenerator(SharedGlobalData.AllSeeds.PromoGemsMazeSeed);

                    int levelGems = (int)Math.Round(gemsGen.GetLevelReward(request.MazeId, request.MazeLevel));
                    int levelPromoGems = (int)Math.Round(promoRewardGen.GetLevelReward(request.MazeId, request.MazeLevel));

                    var levelCoin = SharedGlobalData.CoinPerLevel;


                    //credit money
                    update = update.Inc(p => p.VirtualCurrencies.Coin, levelCoin);
                    //player.VirtualCurrencies.Coin += levelCoin;

                    //credit gems
                    update = update.Inc(p => p.VirtualCurrencies.Gems, levelGems);
                    //player.VirtualCurrencies.Gems += levelGems;

                    int levelExp = (int)Math.Floor(Math.Sqrt(levelGems));

                    //credit experience
                    player.PlayerExp.Point += levelExp;
                    if (player.PlayerExp.Point >= player.PlayerExp.PointToNextLevel)
                    {
                        var remain = player.PlayerExp.Point - player.PlayerExp.PointToNextLevel;

                        player.PlayerExp.ExpLevel += 1;
                        player.PlayerExp.Point = remain;
                        player.PlayerExp.PointToNextLevel += player.PlayerExp.Incr;

                        //update = update
                        //    .Inc(p => p.PlayerExp.ExpLevel, 1)
                        //    .Set(p => p.PlayerExp.Point, remain)
                        //    .Set(p => p.PlayerExp.PointToNextLevel, player.PlayerExp.Incr);
                    }
                    update = update.Set(p => p.PlayerExp, player.PlayerExp);

                    //update bank data
                    var payoutBonus = SharedGlobalData.GetBankPayoutBonus(request.MazeLevel);
                    if (payoutBonus > 0)
                    {
                        //player.Bank.DailyPayout += payoutBonus;
                        update = update.Inc(p => p.Bank.DailyPayout, payoutBonus);

                        var gameNotifications = new GameNotifications()
                        {
                            Id = ItemId.BankPayout,
                            Type = GameNotificationsType.BankPayoutIncrease,
                            Val = payoutBonus,
                        };
                        notifications.Add(gameNotifications);
                    }

                    //update leaderboards

                    var scoreUpdate = new LeaderBoardsScoreUpdate()
                    {
                        TotalPoint = request.TotalPoint,
                        Combo4Ghost = request.Combo4Ghost,
                        SuperCombo4Ghost = request.SuperCombo4
                    };

                    player.GlobalLeaderBoardEntry.UpdateAllScores(scoreUpdate);
                    player.GlobalLeaderBoardEntry.Id = player.Id;
                    player.GlobalLeaderBoardEntry.UserName = player.UserName;


                    update = update
                        .Set(p => p.GlobalLeaderBoardEntry, player.GlobalLeaderBoardEntry);



                    //update stat 

                    //player.GameStatistics.TotalPoint += request.TotalPoint;
                    //player.GameStatistics.GemsChallenge += levelGems;
                    update = update
                        .Inc(p => p.GameStatistics.TotalPoint, request.TotalPoint)
                        .Inc(p => p.GameStatistics.GemsChallenge, levelGems);



                    // update the cache
                    //update leaderboard cache only if authenticated
                    if (player.UserName.IsEmptyString() == false)
                    {
                        var entry = new GlobalLeaderBoardEntry()
                        {
                            Id = player.Id,
                            UserName = player.UserName,
                            TotalPoint = request.TotalPoint,
                            Combo4Ghost = request.Combo4Ghost,
                            SuperCombo4Ghost = request.SuperCombo4
                        };

                        leaderBoardCacheClient.Update(entry);
                    }


                    // update fruit collected stats
                    var productionLine = player.GoodFactory
                        .ProductionLines.FirstOrDefault(l => l.Id == request.MazeLevel);
                    if (productionLine != null && request.LevelFruit > 0)
                    {
                        productionLine.FruitCollected += request.LevelFruit;

                        //check and unlock new production line
                        if (productionLine.FruitCollected >= productionLine.Id * SharedGlobalData.FruitToOpenMultiplier)
                        {
                            if (productionLine.State == ProductionLineState.Close)
                            {
                                productionLine.State = ProductionLineState.Open;


                                var gameNotifications = new GameNotifications()
                                {
                                    Id = (ItemId)productionLine.Id,
                                    Type = GameNotificationsType.NewProductionLine,
                                };
                                notifications.Add(gameNotifications);
                            }
                        }

                        update = update.Set(p => p.GoodFactory.ProductionLines, player.GoodFactory.ProductionLines);

                    }

                    // submit update ###############################################
                    player = await context.Players.FindAndUpdate(filter, update);


                    //give reward to friend
                    if (player.PromoPlayerId.IsNotEmptyString())
                    {
                        var promoPlayer = await context.Players.Find(p => p.Id == player.PromoPlayerId);
                        //give gems
                        if (promoPlayer != null)
                        {

                            var filterBuilder = Builders<PlayerData>.Filter;
                            var promoFilter = filterBuilder.Eq(p => p.Id, promoPlayer.Id);
                            //& filterBuilder.Lt(p=>p.GameStatistics.PromoBankPayoutCount, GlobalData.MaxPromoBankDailyPayoutReward);

                            // var promoFilter = Builders<PlayerData>.Filter.Eq(p => p.Id, promoPlayer.Id);

                            var promoUpdate = Builders<PlayerData>.Update
                                .Inc(p => p.VirtualCurrencies.Gems, levelPromoGems)
                                .Inc(p => p.GameStatistics.PromoGems, levelPromoGems);

                            // give gems promo reward
                            context.Players.Increment(promoFilter, promoUpdate);


                            if (player.BankPayoutGiven == false)
                            {
                                if (player.PlayerExp.ExpLevel >= SharedGlobalData.GiveBankPayoutRewardAtLevel)
                                {
                                    //player.BankPayoutGiven = true;

                                    if (promoPlayer.GameStatistics.PromoBankPayoutCount < SharedGlobalData.MaxPromoBankDailyPayoutReward)
                                    {
                                        var promoBankFilter = filterBuilder.Eq(p => p.Id, promoPlayer.Id)
                                                         & filterBuilder.Lt(p => p.GameStatistics.PromoBankPayoutCount, SharedGlobalData.MaxPromoBankDailyPayoutReward);

                                        var promoBankUpdate = Builders<PlayerData>.Update
                                               .Inc(p => p.GameStatistics.PromoBankPayoutCount, 1)
                                               .Inc(p => p.Bank.DailyPayout, SharedGlobalData.PromoBankDailyPayout)
                                               .Inc(p => p.GameStatistics.PromoBankPayout, SharedGlobalData.PromoBankDailyPayout);

                                        // update promo player bank payout
                                        context.Players.Increment(promoBankFilter, promoBankUpdate);

                                        //update player  BankPayoutGiven
                                        player = await context.Players.FindAndUpdate(player, Builders<PlayerData>.Update.Set(p => p.BankPayoutGiven, true));

                                    }

                                }
                            }


                        }
                    }




                    // player = await context.Players.Update(player);

                    player.Notifications = notifications;

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(player);
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost("UnBlockMaze")]
        public async Task<ApiResponse<PlayerData>> UnBlockMaze([FromBody]UnBlockMazeRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var playerId = player.Id;
                var maze = player.ChallengeMazeList.FirstOrDefault(m => m.MazeId == request.MazeId);
                if (maze != null && (maze.IsOpen == false && player.VirtualCurrencies.Coin >= maze.OpenCost))
                {
                    //player.VirtualCurrencies.Coin -= maze.OpenCost;
                    maze.IsOpen = true;

                    var update = Builders<PlayerData>.Update
                        .Inc(p => p.VirtualCurrencies.Coin, -maze.OpenCost)
                        .Set(p => p.ChallengeMazeList, player.ChallengeMazeList);

                    player = await context.Players.FindAndUpdate(player, update);

                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        ChallengeMazeList = player.ChallengeMazeList,
                        ServerResponse = new ServerResponse() { MazeUnlocked = true }
                    });
                }
                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost("ApplySpeedUp")]
        public async Task<ApiResponse<PlayerData>> ApplySpeedUp([FromBody]ApplyFactoryBoostRequest request)
        {

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            //var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                var invBoost = player.Inventory.AllItems.FirstOrDefault(i => i.ItemId == request.BoostId);
                var challengeBoost = player.ChallengeBoost.FirstOrDefault(s => s.Slot == request.Slot);

                if (invBoost != null && challengeBoost != null)
                {
                    if (invBoost.Count > 0)
                    {
                        invBoost.Count -= 1;
                        challengeBoost.ItemId = request.BoostId;
                        challengeBoost.EndTime = DateTime.UtcNow.AddMinutes(10);
                        challengeBoost.Value = SharedGlobalData.GetBoostValue(request.BoostId);


                        var update = Builders<PlayerData>.Update
                            .Set(x => x.Inventory, player.Inventory)
                            .Set(x => x.ChallengeBoost, player.ChallengeBoost);

                        player = await context.Players.FindAndUpdate(player, update);

                    }



                    //return only modified data
                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        ChallengeBoost = player.ChallengeBoost,
                        Inventory = player.Inventory,
                    });
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }



        [HttpPost("UnlockSingleUpgrade")]
        public async Task<ApiResponse<PlayerData>> UnlockSingleUpgrade([FromBody]UpGradeRequest request)
        {

            if (request == null) return ApiResponse<PlayerData>.CreateError(ApiResponseCode.EmptyRequestData);

            var player = await JwtTools.GetCurrentPlayer(HttpContext.User.Identity as ClaimsIdentity, context);
            // var playerData = await JwtTools.GetCurrentPlayer(request.Token, appSettings.Secret, context);

            if (player != null)
            {
                //check if new upgrade
                var upgrade = player.ChallengeUpgrade.FirstOrDefault(u => u.UpgradeId == request.Id);
                if (upgrade == null)
                {
                    switch (request.Id)
                    {
                        case UpgradeId.FruitTimeIncrease:
                            var itemUpgrade = DefaultUpgradeItem.FruitTimeIncrease;
                            player.ChallengeUpgrade.Add(itemUpgrade);
                            break;
                    }
                }


                upgrade = player.ChallengeUpgrade.FirstOrDefault(u => u.UpgradeId == request.Id);
                if (upgrade != null
                    && upgrade.Cost <= player.VirtualCurrencies.Gems
                    && upgrade.Count < upgrade.MaxCount)
                {
                    player.VirtualCurrencies.Gems -= upgrade.Cost;
                    upgrade.Cost += upgrade.Raison;
                    upgrade.Count += 1;
                    upgrade.Val += upgrade.Inc;

                    var update = Builders<PlayerData>.Update
                        .Set(x => x.ChallengeUpgrade, player.ChallengeUpgrade)
                        .Inc(x => x.VirtualCurrencies.Gems, -upgrade.Cost);


                    player = await context.Players.FindAndUpdate(player, update);


                    return ApiResponse<PlayerData>.CreateSuccess(new PlayerData()
                    {
                        VirtualCurrencies = player.VirtualCurrencies,
                        ChallengeUpgrade = player.ChallengeUpgrade,
                    });
                }

                return ApiResponse<PlayerData>.CreateError(ApiResponseCode.Error);
            }
            return ApiResponse<PlayerData>.CreateError(ApiResponseCode.PlayerNotFound);
        }

    }
}