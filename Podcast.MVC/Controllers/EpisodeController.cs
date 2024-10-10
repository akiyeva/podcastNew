using Microsoft.AspNetCore.Mvc;

namespace Podcast.MVC.Controllers
{
    public class EpisodeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
