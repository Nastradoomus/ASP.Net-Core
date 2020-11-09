using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Components.Helpers;

namespace MVC.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<TimeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IErrorMessage _err;
        public ErrorController(ILogger<TimeController> logger, IWebHostEnvironment env, IErrorMessage err)
        {
            _logger = logger;
            _env = env;
            _err = err;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Error")]
        public IActionResult Error()
        {
            _err.ErrorCode = Response.StatusCode;
            var m = _err.GetMessage();
            var io = DateTime.Now + " Error: " + _err.ErrorCode + " " + m + ", from ip: " + HttpContext.Items["ip"];
            Console.WriteLine(io);
            _logger.LogWarning(io);
            var error = new ErrorViewModel { ResponseCode = _err.ErrorCode, ResponseMessage = m, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(error);
        }
        // Should be handled through exception handling: https://stackoverflow.com/questions/38630076/asp-net-core-web-api-exception-handling
        // Too much hassle for this project 😔. With execption handling we should separate all CRUD operation exceptions since we are replying to GET as
        // a Razor template and POST as a JSON.
    }
}