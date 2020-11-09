using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AboutController : Controller
    {
        public AboutController()
        { }
        [Route("About")]
        [Route("About/Index")]
        [Route("About/Index/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}