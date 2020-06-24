using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aksdotnet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private static DateTime? firstCall = null;

        [HttpGet]
        public string Get()
        {
            // used docker.io/dgkanatsios:0.2-healthfails image
            // if(firstCall == null)
            // {
            //     firstCall = DateTime.UtcNow;
            // }
            // else
            // {
            //     if ((DateTime.UtcNow - firstCall.Value).TotalSeconds > 5)
            //     {
            //         throw new Exception("An awesome crash during health check");
            //     }
            // }

            Console.WriteLine($"GET /health at {DateTime.UtcNow}");
            return "OK";
        }
    }
}
