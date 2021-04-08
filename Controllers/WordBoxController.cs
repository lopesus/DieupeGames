using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DieupeGames.Models.LiteDb;

namespace DieupeGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordBoxController : ControllerBase
    {
        private ILiteDbWordBoxService wordBoxService; 
        public WordBoxController(ILiteDbWordBoxService wordBoxService)
        {
            this.wordBoxService = wordBoxService;
        }


        [HttpGet("")]
        public string Index()
        {
            return "index api";
        }


        [HttpGet("{id}")]
        public string Get([FromRoute] string id)
        {
            return id;
        }
    }
}
