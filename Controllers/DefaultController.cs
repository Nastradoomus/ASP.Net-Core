using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class DefaultController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}