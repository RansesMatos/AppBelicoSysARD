using Microsoft.AspNetCore.Mvc;

namespace BelicoSysApp.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
