using System.Collections.Generic;
using System.Threading;
using LabirunModel.Labirun;
using LabirunServer.DataProtection;
using LabirunServer.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace learnCore
{
    public class MongoDBContext
    {
        private IMongoCollection<MongoStoredKey> dataProtectionCollection;

        private readonly IMongoCollection<PlayerData> playerCollection;
        //private readonly IMongoCollection<GlobalLeaderBoardEntry> leaderBoardCollection;
        private readonly IMongoCollection<PromoCode> promoCodeCollection;
        private readonly IMongoCollection<ServerCounter> serverCounterCollection;

        public PlayerDataRepository Players { get; set; }
        //LeaderBoardRepository LeaderBoard { get; set; }
        public PromoCodeRepository PromoCode { get; set; }
        public ServerCounterRepository Counters { get; set; }


        private readonly AppSettings appSettings;

        public MongoDBContext(IOptions<AppSettings> options)
        {
            appSettings = options.Value;


            var url2 = new MongoUrl(appSettings.MongoServer);
            var clientSettings = MongoClientSettings.FromUrl(url2);
            clientSettings.SslSettings = new SslSettings();
            clientSettings.SslSettings.CheckCertificateRevocation = false;
            //clientSettings.UseSsl = true;
            clientSettings.UseTls = true;
            //clientSettings.VerifySslCertificate = false;
            clientSettings.AllowInsecureTls = true;


            //var mongoClient = new MongoClient(url);
            var client = new MongoClient(clientSettings);

            //var client = new MongoClient(appSettings.MongoServer);
            var database = client.GetDatabase(appSettings.MongoDatabase);
            //for data protection key 
            dataProtectionCollection = database.GetCollection<MongoStoredKey>("DataProtection");


             playerCollection = database.GetCollection<PlayerData>("PlayerData");
            //leaderBoardCollection = database.GetCollection<GlobalLeaderBoardEntry>("LeaderBoard");
            promoCodeCollection = database.GetCollection<PromoCode>("PromoCode");
            serverCounterCollection = database.GetCollection<ServerCounter>("ServerCounter");

            CreatePromoCodeIndex();
            //CreatePlayerDataIndex();
            //CreateCreatedMazeIndex();

            //Players = new PlayerDataRepository(playerCollection);
            PromoCode = new PromoCodeRepository(promoCodeCollection);
            //LeaderBoard = new LeaderBoardRepository(leaderBoardCollection);
            Counters = new ServerCounterRepository(serverCounterCollection);
        }

        private void CreatePromoCodeIndex()
        {
            var indexModels = new List<CreateIndexModel<PromoCode>>();
            var indexOptions = new CreateIndexOptions() {  };
            var builder = Builders<PromoCode>.IndexKeys;

            indexModels.Add(new CreateIndexModel<PromoCode>(builder.Ascending(p => p.Id), indexOptions));
            indexModels.Add(new CreateIndexModel<PromoCode>(builder.Ascending(p => p.Code), indexOptions));

            promoCodeCollection.Indexes.CreateMany(indexModels);
        }

        private void CreatePlayerDataIndex()
        {
            var indexModels = new List<CreateIndexModel<PlayerData>>();
            var indexOptions = new CreateIndexOptions() { Unique = false, };
            var builder = Builders<PlayerData>.IndexKeys;

            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.DeviceId), indexOptions));
            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.FaceBookId), indexOptions));
            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.KartRidgeId), indexOptions));
            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.SteamId), indexOptions));
            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.UserName), indexOptions));
            indexModels.Add(new CreateIndexModel<PlayerData>(builder.Ascending(p => p.Email), indexOptions));

            playerCollection.Indexes.CreateMany(indexModels);
        }

        private void CreateCreatedMazeIndex()
        {
            //var indexModels = new List<CreateIndexModel<CreatedMaze>>();
            //var indexOptions = new CreateIndexOptions() { Unique = false, };
            //var builder = Builders<CreatedMaze>.IndexKeys;

            //indexModels.Add(new CreateIndexModel<CreatedMaze>(builder.Ascending(p => p.SystemName), indexOptions));
            ////indexModels.Add(new CreateIndexModel<CreatedMaze>(builder.Ascending(p => p.PlayCount), indexOptions));
            ////indexModels.Add(new CreateIndexModel<CreatedMaze>(builder.Ascending(p => p.Rating), indexOptions));

            //promoCmodeCollection.Indexes.CreateMany(indexModels);
        }

        public IMongoCollection<MongoStoredKey> GetProtectionCollection()
        {
            return dataProtectionCollection;
        }


        //public List<PlayerData> Get() =>
        //    playerCollection.Find(book => true).ToList();

        //public PlayerData Get(string id) =>
        //    playerCollection.Find<PlayerData>(book => book.Id == id).FirstOrDefault();

        //public PlayerData Create(PlayerData book)
        //{
        //    playerCollection.InsertOne(book);
        //    return book;
        //}
        //public void  SaveList(List<PlayerData> book)
        //{
        //    playerCollection.InsertMany(book);
        //}

        //public void Update(string id, PlayerData bookIn) =>
        //    playerCollection.ReplaceOne(book => book.Id == id, bookIn);

        //public void Remove(PlayerData bookIn) =>
        //    playerCollection.DeleteOne(book => book.Id == bookIn.Id);

        //public void Remove(string id) =>
        //    playerCollection.DeleteOne(book => book.Id == id);
    }
}