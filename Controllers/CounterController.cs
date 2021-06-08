using System.Threading.Tasks;
using DieupeGames.Models.LiteDb;
using learnCore;
using Microsoft.AspNetCore.Mvc;

namespace DieupeGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly MongoDBContext context;
        public CounterController(MongoDBContext context )
        {
            this.context = context;
        }


        [HttpGet("")]
        public async Task<long> Index()
        {
            var counter = await context.Counters.GetNextCounter();
            return counter.Counter;
        }


        //[HttpGet("{id}")]
        //public string Get([FromRoute] string id)
        //{
        //    return id;
        //}
    }
}