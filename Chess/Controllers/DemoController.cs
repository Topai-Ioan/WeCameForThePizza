using Microsoft.AspNetCore.Mvc;

namespace Chess.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
