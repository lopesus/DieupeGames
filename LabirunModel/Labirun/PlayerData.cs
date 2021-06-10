using System;
using System.Collections.Generic;
using System.Linq;
using LabirunModel.Config;
using LabirunModel.Labirun.Enums;
using LabirunModel.Labirun.Response;
using LabirunModel.Tools;

namespace LabirunModel.Labirun
{
    public partial class PlayerData
    {
        public string Id { get; set; }
        public string FaceBookId { get; set; }
        public string KartRidgeId { get; set; }
        public string SteamId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
        public string SessionId { get; set; }

        public PlayerProfile Profile { get; set; }

        public string DeviceId { get; set; }

        // public string PromoCode { get; set; }
        public string PromoPlayerId { get; set; }
       
        public DateTime NextDailyRewardTime { get; set; }
        public bool BankPayoutGiven { get; set; }


        public VirtualCurrencies VirtualCurrencies { get; set; }
        public PlayerCustomMaze PlayerMazes { get; set; }

        public PlayerExp PlayerExp { get; set; }
        //public string Exp { get; set; }

        public GameStatistics GameStatistics { get; set; }
        public GlobalLeaderBoardEntry GlobalLeaderBoardEntry { get; set; }

        public List<Maze> ChallengeMazeList { get; set; }

        public List<FactoryBoost> ChallengeBoost { get; set; }

        //public List<LevelCompleted> MazeLevelCompleted { get; set; }
        //public List<LevelCompleted> FruitCollected { get; set; }
        //public PromoLevelCount PromoCounter { get; set; }

        //public bool CanGiveSpeedBooster;


        // public bool LegendaryStartPromoAvailable { get; set; }
        public bool HasLegendaryStart { get; set; }
        public GoodFactory GoodFactory { get; set; }

        public Mine Mine { get; set; }

        public Bank Bank { get; set; }

        public Inventory Inventory { get; set; }
        // public LeaderBoardsScore LeaderBoards { get; set; }

        public List<TournamentInfos> PlayerTournament { get; set; }

        public List<ItemUpgrade> ChallengeUpgrade { get; set; }

        //PromoProgressInfos?: PromoProgressInfos { get; set; }

        //public AllSeeds AllSeeds { get; set; }

        // not mapped 

        public List<GameNotifications> Notifications { get; set; }
        public ConfirmPurchaseResponse PurchaseResponse { get; set; }

        public bool Success { get; set; }
        //public ShopPromo ShopPromo { get; set; }

        //used for partial response
        public TournamentScore TournamentScore { get; set; }
        public ServerResponse ServerResponse { get; set; }

        public bool IsTester { get; set; }

        public DateTime LastLogin { get; set; }


        public void Initialize()
        {
            HasLegendaryStart = false;
            PromoPlayerId = null;
            GameStatistics = new GameStatistics();
            BankPayoutGiven = false;

            VirtualCurrencies = new VirtualCurrencies
            {
                Coin = SharedGlobalData.InitialCoin,
                Gems = SharedGlobalData.InitialGems
            };

            PlayerMazes = new PlayerCustomMaze();

            PlayerExp = new PlayerExp
            {
                ExpLevel = 1,
                Point = 0,
                Incr = 1000,
                U1 = 1000,
                PointToNextLevel = 1000
            };

            Bank = new Bank
            {
                BuildingId = BuildingId.Bank,
                IsActive = false,
                DailyPayout = SharedGlobalData.InitialBankDailyPayout
            };

            GoodFactory = new GoodFactory
            {
                BuildingId = BuildingId.GoodFactory,
                Status = ProductionStatus.Idle,
                ProductionTime = ProductionTime.Hour1,
                ProductionBonus = 0,
                UpgradeCount = 0
            };

            // init production line 
            foreach (ItemId itemId in SharedGlobalData.ProductItemsList)
            {
                GoodFactory.ProductionLines.Add(new ProductionLine
                {
                    Id = (int)itemId,
                    State = ProductionLineState.Close,
                    FruitCollected = 0,
                    ProdBonus = 0,
                    UpgradeCount = 0,
                    Product = new Product
                    {
                        Id = itemId,
                        UpgradeCount = 0,
                        ProdPerHour = SharedGlobalData.ProductInitialProductionPerHour,
                        UnitPrice = 0.1f * (int)itemId
                    }
                });
            }


            GlobalLeaderBoardEntry = new GlobalLeaderBoardEntry()
            {
                Id = Id,
                UserName = UserName,
            };

            //LeaderBoards=new LeaderBoardsScore();

           
            Inventory = new Inventory();

            #region init inventory

            // init goods 
            foreach (ItemId itemId in SharedGlobalData.ProductItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = SharedGlobalData.InitialProductCount,
                    Category = ItemCategory.Product,
                    Type = ItemType.Product
                });
            }

            // init meal 
            foreach (ItemId itemId in SharedGlobalData.MealsItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = 0,
                    Category = ItemCategory.Meal,
                    Type = ItemType.Meal
                });
            }

            // init daily boost 
            foreach (ItemId itemId in SharedGlobalData.FactoryDailyBoostItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = 10,
                    Category = ItemCategory.ProductionBooster,
                    Type = ItemType.ProdDailyBoost,
                    AcquisitionType = ItemAcquisition.CanBeBought,
                    Duration = ItemDuration.Day,
                });
            }


            // init monthly boost 
            foreach (ItemId itemId in SharedGlobalData.FactoryMonthlyBoostItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = 0,
                    Category = ItemCategory.ProductionBooster,
                    Type = ItemType.ProdMonthlyBoost,
                    AcquisitionType = ItemAcquisition.CanBeBought,
                    Duration = ItemDuration.Month,
                });
            }


            // init speed up  
            foreach (ItemId itemId in SharedGlobalData.SpeedUpItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = SharedGlobalData.InitialItemCount,
                    Category = ItemCategory.SpeedUp,
                    Type = ItemType.HeroSpeedUp,
                    AcquisitionType = ItemAcquisition.CanBeBought,
                });
            }

            // init slowdown up  
            foreach (ItemId itemId in SharedGlobalData.SlowDownItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = SharedGlobalData.InitialItemCount,
                    Category = ItemCategory.SpeedUp,
                    Type = ItemType.EnemySlowDown,
                    AcquisitionType = ItemAcquisition.CanBeBought,
                });
            }

            // init afraid up  
            foreach (ItemId itemId in SharedGlobalData.AfraidItemsList)
            {
                Inventory.AllItems.Add(new InventoryItem
                {
                    ItemId = itemId,
                    Count = SharedGlobalData.InitialItemCount,
                    Category = ItemCategory.SpeedUp,
                    Type = ItemType.EnemyAfraid,
                    AcquisitionType = ItemAcquisition.CanBeBought,
                });
            }

            #endregion

            #region challenge upgrade

            ChallengeUpgrade = new List<ItemUpgrade>()
            {
                DefaultUpgradeItem.HeroSpeedIncrease,
                DefaultUpgradeItem.AfraidTimeIncrease,
                DefaultUpgradeItem.AfraidSpeedDecrease,
                DefaultUpgradeItem.FruitTimeIncrease,
            };

            #endregion

            #region challenge boost

            ChallengeBoost = new List<FactoryBoost>()
            {
                new FactoryBoost()
                {
                    Slot = BoostSlot.HeroSpeedUp, EndTime = DateTime.UtcNow
                },
                new FactoryBoost()
                {
                    Slot = BoostSlot.EnemySlowDown, EndTime = DateTime.UtcNow
                },
                new FactoryBoost()
                {
                    Slot = BoostSlot.AfraidUp, EndTime = DateTime.UtcNow
                },
            };

            #endregion


            ChallengeMazeList = new List<Maze>();
            for (int i = 1; i <= 54; i++)
            {
                var maze = new Maze
                {
                    MazeId = i,
                    IsOpen = false,
                    Level = 1,
                    OpenCost = i * SharedGlobalData.InitialMazeOpenCost
                };
                if (i == 1)
                {
                    maze.IsOpen = true;
                    maze.OpenCost = 0;
                }
                ChallengeMazeList.Add(maze);
            }
        }

        public void AddToInventoryItem(InventoryItem item)
        {
            switch (item.ItemId)
            {
                case ItemId.CoinPack:
                    VirtualCurrencies.Coin += item.Count;
                    break;
                case ItemId.GemPack:
                    VirtualCurrencies.Gems += item.Count;
                    break;
                default:
                    var invItem = Inventory.AllItems.FirstOrDefault(i => i.ItemId == item.ItemId);
                    if (invItem != null) invItem.Count += item.Count;
                    break;
            }
        }

        public void AddToInventoryItem(ItemId itemId, int count)
        {
            var invItem = Inventory.AllItems.FirstOrDefault(i => i.ItemId == itemId);
            if (invItem != null) invItem.Count += count;
        }



        public void UpdateData(PlayerData newData)
        {
            if (newData == null) return;

            UserName = newData.UserName ?? UserName;
            FaceBookId = newData.FaceBookId ?? FaceBookId;
            Id = newData.Id ?? Id;

            //Token = newData.Token ?? Token;
            if (newData.Token.IsNotEmptyString())
            {
                Token = newData.Token;
            }

            if (newData.NextDailyRewardTime.IsEmptyDate()==false)
            {
                NextDailyRewardTime = newData.NextDailyRewardTime;
            }

            SessionId = newData.SessionId ?? SessionId;

            VirtualCurrencies = newData.VirtualCurrencies ?? VirtualCurrencies;
            PlayerMazes = newData.PlayerMazes ?? PlayerMazes;
            GlobalLeaderBoardEntry = newData.GlobalLeaderBoardEntry ?? GlobalLeaderBoardEntry;

            PromoPlayerId = newData.PromoPlayerId ?? PromoPlayerId;
            BankPayoutGiven = newData.BankPayoutGiven || BankPayoutGiven;

            HasLegendaryStart = newData.HasLegendaryStart;
            
            //PromoRewardGiven=newData.PromoRewardGiven??PromoRewardGiven;
            GameStatistics = newData.GameStatistics ?? GameStatistics;
            ChallengeMazeList = newData.ChallengeMazeList ?? ChallengeMazeList;
            //MazeLevelCompleted = newData.MazeLevelCompleted ?? MazeLevelCompleted;

            GoodFactory = newData.GoodFactory ?? GoodFactory;
            Mine = newData.Mine ?? Mine;
            Bank = newData.Bank ?? Bank;

            Inventory = newData.Inventory ?? Inventory;
            ChallengeBoost = newData.ChallengeBoost ?? ChallengeBoost;
            PlayerExp = newData.PlayerExp ?? PlayerExp;


            ChallengeUpgrade = newData.ChallengeUpgrade ?? ChallengeUpgrade;
            // ensure upgrade added later are shown
            var up = ChallengeUpgrade.FirstOrDefault(u => u.UpgradeId == UpgradeId.FruitTimeIncrease);
            if (up == null)
            {
                ChallengeUpgrade.Add(DefaultUpgradeItem.FruitTimeIncrease);
            }


            //PromoProgressInfos=newData.PromoProgressInfos??PromoProgressInfos;
            //AllSeeds = newData.AllSeeds ?? AllSeeds;


            Notifications = newData.Notifications ?? Notifications;
            PlayerTournament = newData.PlayerTournament ?? PlayerTournament;
        }


        public override string ToString()
        {
            return //$"{nameof(PlayerId)}: {PlayerId}\r\n " +
                $"{nameof(Id)}: {Id}\r\n " +
                $"{nameof(UserName)}: {UserName}\r\n " +
                $"{nameof(PromoPlayerId)}: {PromoPlayerId}\r\n" +
                $"{nameof(VirtualCurrencies)}: {VirtualCurrencies}\r\n " +
                $"{nameof(PlayerExp)}: {PlayerExp}\r\n" +
                $"{nameof(BankPayoutGiven)}: {BankPayoutGiven}\r\n" +
                $"{nameof(GameStatistics)}: {GameStatistics}\r\n" +
                $"{nameof(Bank)}: {Bank}\r\n " +
                $"{nameof(GoodFactory)}: {GoodFactory}\r\n" +

                //$"{nameof(Inventory)}: {Inventory}\r\n" +
                $"{nameof(Token)}: {Token}\r\n"
                ;
        }
    }
}