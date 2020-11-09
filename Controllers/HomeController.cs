using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
   public class HomeController : Controller
    {
        public HomeController()
        { }

        [Route("Home")]
        [Route("Home/Index")]
        [Route("Home/Index/{id:int:maxlength(3):minlength(1)?}")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/Privacy")]
        [Route("Home/Privacy/{id?}")]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}