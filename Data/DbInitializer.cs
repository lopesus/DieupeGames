using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using LabirunModel.Labirun;
using LabirunModel.Labirun.Enums;

namespace learnCore.Models
{
    public static class DbInitializer
    {
        [Conditional("DEBUG")]
        public static async void GenerateMongoPlayer(MongoDBContext dbContext, int playerCount = 10)
        {
            var playerList000 = new List<PlayerData>();

            for (int i = 0; i < playerCount; i++)
            {

                var playerExp = new PlayerExp() { ExpLevel = 1, Incr = 1000, Point = 0, PointToNextLevel = 1000, U1 = 1000 };
                var userName = "player_" + i;
                var player = new PlayerData()
                {
                    //Id = Guid.NewGuid().ToString(),
                    //UserName = userName,
                    DeviceId = Guid.NewGuid().ToString(),
                    //PromoCode = Guid.NewGuid().ToString(),
                    Profile = new PlayerProfile()
                    {
                        UserName = userName,
                        PassWord = "gis",
                        IsAnonymousAccount = false,
                    },
                    Email = $"player_{i}@email.com",
                    NextDailyRewardTime = DateTime.UtcNow,
                    PromoPlayerId = 7.ToString(),
                    GameStatistics = new GameStatistics()
                    {
                        //PlayerCount = 0,
                        PromoCount = 0,
                        TotalPoint = 0,
                        PromoGems = 0,
                        TournamentFastest = 0
                    },
                    VirtualCurrencies = new VirtualCurrencies() { Coin = 10 + i, Gems = 100 },
                    BankPayoutGiven = false,
                    //CanGiveSpeedBooster = false,
                    HasLegendaryStart = false,
                    PlayerExp = playerExp,
                };
                player.Initialize();

                playerList000.Add(player);
            }

            try
            {
                await dbContext.Players.SaveList(playerList000);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        //[Conditional("DEBUG")]
        //public static void Initialize(LabirunPlayerDataContext context)
        //{
        //    int playerCount = 10;
        //    //context.Database.EnsureDeleted();
        //    context.Database.EnsureCreated();

        //    //context.Database.Migrate();

        //    // Look for any students.
        //    if (context.PlayerData.Any())
        //    {
        //        return;   // DB has been seeded
        //    }

        //    GeneratePlayer(context, playerCount);
        //}

        //public static void GeneratePlayer(LabirunPlayerDataContext context, int playerCount)
        //{
        //    var playerList = new List<PlayerData>();

        //    for (int i = 0; i < playerCount; i++)
        //    {
        //        var user = new User
        //        {

        //        };

        //        var playerExp = new PlayerExp() { ExpLevel = 1, Incr = 1000, Point = 0, PointToNextLevel = 1000, U1 = 1000 };
        //        var userName = "player_" + i;
        //        var player = new PlayerData()
        //        {
        //            UserName = userName,
        //            DeviceId = Guid.NewGuid().ToString(),
        //            Profile = new PlayerProfile()
        //            {
        //                UserName = userName,
        //                PassWord = "gis",
        //                IsAnonymousAccount = false,
        //            },
        //            Email = $"player_{i}@email.com",
        //            NextDailyRewardTime = 111,
        //            PromoPlayerId = 7,
        //            GameStatistics = new GameStatistics()
        //            {
        //                BoosterGiven = 0,
        //                BoosterToApply = 0,
        //                PlayerCount = 0,
        //                PromoBankInterestToApply = 0,
        //                PromoCount = 0,
        //                PromoDaily100ToApply = 0,
        //                PromoGemsToApply = 0,
        //                TotalBankInterestGiven = 0,
        //                TotalPoint = 0,
        //                TotalPromoDaily100Given = 0,
        //                PromoGems = 0,
        //                TournamentFastest = 0
        //            },
        //            VirtualCurrencies = new VirtualCurrencies() { Coin = 10 + i, Gems = 100 },
        //            BankPayoutGiven = false,
        //            CanGiveSpeedBooster = false,
        //            LegendaryStartPromoAvailable = true,
        //            PlayerExp = playerExp,
        //        };
        //        var serialize = JsonSerializer.Serialize(playerExp);

        //        #region MazeLevelCompleted
        //        player.MazeLevelCompleted = new List<LevelCompleted>();
        //        for (int j = 0; j < 25; j++)
        //        {
        //            player.MazeLevelCompleted.Add(new LevelCompleted()
        //            {
        //                Count = 0,
        //                Level = j + 1
        //            });
        //        }
        //        #endregion

        //        #region ChallengeMazeList

        //        var listMaze = new List<Maze>();
        //        for (int j = 1; j <= 54; j++)
        //        {
        //            listMaze.Add(new Maze()
        //            {
        //                MazeId = j,
        //                IsOpen = false,
        //                Level = 1,
        //                OpenCost = j * 10
        //            });
        //        }

        //        player.ChallengeMazeList = listMaze;
        //        #endregion

        //        #region Inventory
        //        player.Inventory = new Inventory();
        //        foreach (ItemId value in Enum.GetValues(typeof(ItemId)))
        //        {
        //            player.Inventory.AllItems.Add(new InventoryItem()
        //            {
        //                ItemId = value,
        //                AcquisitionType = ItemAcquisition.CanBeBought,
        //                Category = ItemCategory.Meal,
        //                Count = 100,
        //                Duration = ItemDuration.Day,
        //                PartCount = 200,
        //                Type = ItemType.Chest,
        //                Value = 50,
        //            });
        //        }

        //        #endregion

        //        #region GoodFactory
        //        player.GoodFactory = new GoodFactory()
        //        {
        //            BuildingId = BuildingId.GoodFactory,
        //            UpgradeCount = 0,
        //            ProductionBonus = 0,
        //            StartTimeUtcMilliSec = 0,
        //            RemainTime = 0,
        //            ProductionTime = ProductionTime.Hour1,
        //            Status = ProductionStatus.Idle,
        //        };
        //        for (int j = 0; j < 25; j++)
        //        {
        //            var prodLine = new ProductionLine()
        //            {
        //                Id = j,
        //                State = ProductionLineState.Close,
        //                ProdBonus = 0,
        //                UpgradeCount = 0,
        //                Product = new Product()
        //                {
        //                    Id = (ItemId)(j + 1),
        //                    UpgradeCount = 0,
        //                    ProdPerHour = 10,
        //                    UnitPrice = 1 + j
        //                }
        //            };


        //            player.GoodFactory.ProductionLines.Add(prodLine);
        //        }

        //        #endregion

        //        #region Bank
        //        player.Bank = new Bank()
        //        {
        //            BuildingId = BuildingId.Bank,
        //            IsActive = false,
        //            Deposit = 10,
        //            Interest = 10,
        //            BonusInterest = 10,
        //            StartTime = 10,
        //            PayoutEvery = 4,
        //        };
        //        #endregion


        //        context.Users.Add(user);
        //        context.PlayerData.Add(player);
        //        //playerList.Add(player);
        //    }

        //    try
        //    {
        //        context.SaveChanges();
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //        throw;
        //    }
        //}
    }
}