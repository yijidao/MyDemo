using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StationStatusController : ControllerBase
    {
        private readonly ILogger<StationStatusController> _logger;

        public StationStatusController(ILogger<StationStatusController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Dictionary<string, byte>[] Get()
        {
            var dic = new Dictionary<string, byte>();
            var count = 0;
            var random = new Random();
            var n = random.Next();
            while (count < 300)
            {
                if(n % 2 == 0)
                {
                    dic.Add($"P{count.ToString().PadLeft(3, '0')}", 0);
                    
                }
                else
                {
                    dic.Add($"P{count.ToString().PadLeft(3, '0')}", 1);
                }
                count++;
                n = random.Next();
            }
            return new Dictionary<string, byte>[] { dic };
        }

    }
}
