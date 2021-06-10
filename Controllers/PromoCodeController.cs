using System.Threading.Tasks;
using DieupeGames.Data.Mongo;
using DieupeGames.Helpers;
using LabirunModel.Labirun;
using Microsoft.AspNetCore.Mvc;

namespace DieupeGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly MongoDBContext context;
        public PromoCodeController(MongoDBContext context)
        {
            this.context = context;
        }


        [HttpGet("")]
        public async Task<long> Index()
        {
            var counter = await context.Counters.GetNextCounter();
            return counter.Counter;
        }


        [HttpGet("CreateCodeForPlayerId/{id}")]
        public async Task<string> CreateCodeForPlayerId([FromRoute] string id)
        {
            var counter = await context.Counters.GetNextCounter();
            //+100 000 to ensure a 4 letter base 36 code
            var code = Base36.Encode(counter.Counter + 100000);
            await context.PromoCode.Create(new PromoCode() { PlayerId = id, Code = code });
            return code;
        }
        [HttpGet("GetPlayerId/{code}")]
        public async Task<string> GetPlayerId([FromRoute] string code)
        {
            var promoCode = await context.PromoCode.Find(p => p.Code == code);
            return promoCode.PlayerId;
        }


    }
}