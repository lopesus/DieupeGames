//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using LabirunModel.Labirun;
//using LabirunModel.Labirun.Request;
//using LabirunModel.Labirun.Response;
//using LabirunServer.Services;
//using learnCore;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;

//namespace LabirunServer.Controllers.Labirun
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CreatedMazeCacheController : ControllerBase
//    {
//        private readonly MongoDBContext context;
//        private readonly AppSettings appSettings;
//        private IDistributedCache cache;
//        private ILogger<CreatedMazeCacheController> logger;
//        private readonly ICreatedMazeCacheClient service;

//        public CreatedMazeCacheController(MongoDBContext context, IOptions<AppSettings> appSettings, ICreatedMazeCacheClient service, IDistributedCache cache, ILogger<CreatedMazeCacheController> logger)
//        {
//            this.service = service;
//            this.context = context;
//            this.appSettings = appSettings.Value;
//            this.cache = cache;
//            this.logger = logger;
//        }


//        [HttpPost("GetRange")]
//        public async Task<ApiResponse<PublicMazeResponse>> GetRange([FromBody] RangeRequest request)
//        {
//            try
//            {
//                var result = await service.GetRange(request);
//                return ApiResponse<PublicMazeResponse>.CreateSuccess(result);
//            }
//            catch (Exception e)
//            {
//                return ApiResponse<PublicMazeResponse>.CreateError(ApiResponseCode.Error);
//            }
//        }



//        [HttpPost("Update")]
//        public async Task<ApiResponse<string>> Update([FromBody] CreatedMaze request)
//        {
//            try
//            {
//                var result = await service.Update(request);
//                return ApiResponse<string>.CreateSuccess(result);
//            }
//            catch (Exception e)
//            {
//                return ApiResponse<string>.CreateError(ApiResponseCode.Error);
//            }
//        }


//        [HttpPost("Initialize")]
//        public async Task<BasicResponse> Initialize()
//        {
//            try
//            {
//                var temp = await context.PublicMazes.GetAll();
//                var entries = temp.ToList();

//                var rangeRequest = new RangeRequest()
//                {
//                    CreatedMazes = entries,
//                };

//                //no await is correct here, fire and forget 
//                logger.LogInformation($"initializing leader with{entries.Count} entries ");
//                service.Initialize(rangeRequest);

//                return new BasicResponse();


//            }
//            catch (Exception e)
//            {
//                return new BasicResponse(); 
//            }

//        }
//    }
//}