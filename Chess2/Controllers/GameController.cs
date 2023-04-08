using Microsoft.AspNetCore.Mvc;

namespace Chess.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
