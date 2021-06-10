using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSwag.Annotations;

namespace DieupeGames.Controllers.PacRun
{
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiIgnore]
    public class FacebookController : ControllerBase
    {
        private readonly MongoDBContext context;
        private readonly AppSettings appSettings;
        private IDistributedCache cache;
        private ILogger<PlayerController> logger;

        public FacebookController(MongoDBContext context, IOptions<AppSettings> appSettings, IDistributedCache cache, ILogger<PlayerController> logger)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.cache = cache;
            this.logger = logger;
        }

        [HttpPost("Deauthorize")]
        public async Task<ActionResult> Deauthorize()
        {
            return Ok();
        }

        [HttpPost("DataDeletion")]
        public async Task<ActionResult> DataDeletion()
        {
            return Ok();
        }

        [HttpPost("RedirectURI")]
        public async Task<ActionResult> RedirectURI()
        {
            return Ok();
        }

        [HttpGet("PaymentsObject")]
        public async Task<ActionResult<string>> PaymentsObjectGet()
        {
            if (Request.Query.TryGetValue("hub.challenge", out var values))
            {
                if (StringValues.IsNullOrEmpty(values)==false)
                {
                    var val = values.ToArray()[0];
                    return val;

                }
            }

            return BadRequest();
        }



        [HttpPost("PaymentsObject")]
        public async Task<ActionResult> PaymentsObjectPost()
        {
            return Ok();

            //string color = Request.Query["color"];
            //if (Request.Query.TryGetValue("color", out var colorValue))
            //{
            //    //DoSomething(colorValue);
            //}
            //var str = Request.Query;
            //foreach (var que in str)
            //{
            //    var key = que.Key;
            //    StringValues val = que.Value;
            //}
            //var query = QueryHelpers.ParseQuery(str.ToString());
            //return "ok";
        }


    }
}