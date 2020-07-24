using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarPositionController : ControllerBase
    {
        private readonly ILogger<CarPositionController> _logger;

        public CarPositionController(ILogger<CarPositionController> logger)
        {
            _logger = logger;
            CreateLinePaths();
        }

        private static Dictionary<string, Point[]> LinePaths { get; set; }

        private void CreateLinePaths()
        {
            if (LinePaths != null) return;
            LinePaths = new Dictionary<string, Point[]>();
            var points1 = new Point[] { new Point(0, 0), new Point(100, 0), new Point(110, 40), new Point(140, 60) };
            LinePaths.Add("L1", points1);
        }

        private static Dictionary<string, Point> CarPosition { get; set; } = new Dictionary<string, Point>();

        private Random GoRandom { get; set; } = new Random();


        private Point Go(string line)
        {
            if (line == "L1")
            {
                var p = CarPosition["C1"];
                if (p.X < 100)
                {
                    return new Point { X = GoRandom.Next(p.X, 101), Y = 0 };
                }
                else if (p.X < 110)
                {
                    // k = 40 / 10
                    var p2 = new Point();
                    p2.X = GoRandom.Next(p.X, 111);
                    p2.Y = (p2.X - 100) * 4;
                    return p2;
                }
                else if (p.X < 140)
                {
                    // k = 20/30
                    var p3 = new Point();
                    p3.X = GoRandom.Next(p.X, 141);
                    p3.Y = ((p3.X - 110) * 2 / 3) + 40;
                    return p3;
                }
                else
                {
                    return new Point(0, 0);
                }

            }
            else
            {
                return new Point(0, 0);
            }
        }

        [HttpGet]
        public Dictionary<string, string>[] Get()
        {
            if (CarPosition.ContainsKey("C1"))
            {
                var p = CarPosition["C1"];
                var p2 = Go("L1");
                CarPosition["C1"] = p2;
            }
            else
            {
                CarPosition.Add("C1", new Point(0, 0));
            }

            return new Dictionary<string, string>[] { CarPosition.ToDictionary(x => x.Key, x => $"{x.Value.X},{x.Value.Y}") };

        }
    }
}
