using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DieupeGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordBoxController : ControllerBase
    {
        public WordBoxController( )
        {
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
