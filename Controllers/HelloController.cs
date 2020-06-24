using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aksdotnet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            Console.WriteLine($"GET /hello at {DateTime.UtcNow} on version {Program.Version}");
            return $"Hello from {Dns.GetHostName()} on version {Program.Version}";
        }
    }
}
