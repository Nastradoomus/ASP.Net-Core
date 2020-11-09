using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MVC.Models;
using MVC.Components.Helpers;

namespace MVC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TimeController : Controller
    {
        private readonly ILogger<TimeController> _logger;

        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _config;
        
        private readonly IErrorMessage _err;

        public TimeController(ILogger<TimeController> logger, IWebHostEnvironment env, IConfiguration config, IErrorMessage err)
        {
            _logger = logger;
            _env = env;
            _config = config;
            _err = err;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("~/api/time")]
        [HttpGet("api")]
        public IActionResult GetTime([FromServices] IWebHostEnvironment env)
        {
            var result = new ObjectResult(DateTime.Now.AddSeconds(TimeOffsetModel.offset));
            var io = result.Value.ToString() + " 🔌 Request from ip: " + HttpContext.Items["ip"];
            Console.WriteLine(io);
            _logger.LogInformation(io);
            result.StatusCode = 200;
            //result.Value = result;
            return Ok(result);
        }

        [HttpPost("~/api/time")]
        [HttpPost("api")]
        public IActionResult Error([FromServices] IWebHostEnvironment env) {
            // If you call post without an offset we return statuscode 403 (Not allowed) and tell the user to reference api correctly
            _err.ErrorCode = 403;
            var m = _err.GetMessage();
            var i = "Please provide offset through " + HttpContext.Request.Path + "/{offset:int}";
            var io = DateTime.Now + " Error: " + _err.ErrorCode + " " + m + " Information: " + i + ", from ip: " + HttpContext.Items["ip"];
            Console.WriteLine(io);
            _logger.LogWarning(io);
            var error = new ObjectResult(new ApiErrorModel {Message = m, Information = i, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            error.StatusCode = _err.ErrorCode;
            return StatusCode(403, error);
        // Should be handled through exception handling: https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
        // Too much hassle for this project 😔. With execption handling we should separate all CRUD operation exceptions since we are replying to GET as
        // a Razor template and /api/ GET and POST as a JSON.
        }

        [HttpPost("~/api/time/{id:int}")]
        [HttpPost("api/{id:int}")]
        public IActionResult SetTimeOffset([FromServices] IWebHostEnvironment env, int id) {
            // Move current to previous
            TimeOffsetModel.previousOffset = TimeOffsetModel.offset;
            // Cast new offset from id
            TimeOffsetModel.offset = id;
            // Log request
            var io = "TimeController.cs: Offset: " + TimeOffsetModel.offset;
            Console.WriteLine(io);
            _logger.LogInformation(io);
            // Put to a new object
            var offset = new Offset { previous = TimeOffsetModel.previousOffset, current = TimeOffsetModel.offset };
            var result = new ObjectResult(offset);
            result.StatusCode = 200;
            return Ok(result);
        }
    }
    public class Offset {
        public int previous {get; set;}
        public int current {get; set;}
    }
}
