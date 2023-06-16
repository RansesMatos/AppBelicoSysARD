using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BelicoSysApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiServicePertrecho _apiService;

        public HomeController(ILogger<HomeController> logger,IApiServicePertrecho apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}