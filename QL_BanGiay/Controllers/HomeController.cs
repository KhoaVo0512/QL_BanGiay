using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Models;
using System.Diagnostics;

namespace QL_BanGiay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoe _ShoeRepo;

        public HomeController(ILogger<HomeController> logger, IShoe shoe)
        {
            _logger = logger;
            _ShoeRepo = shoe;
        }

        public IActionResult Index()
        {
            ViewBag.ItemsVans = _ShoeRepo.GetItemsVans();
            ViewBag.ItemsConverse = _ShoeRepo.GetItemsConverse();
            ViewBag.ItemsAdidas = _ShoeRepo.GetItemsAdidas();
            ViewBag.ItemsNike = _ShoeRepo.GetItemsNike();
            return View();
        }

        [Route("{url}")]
        [HttpGet]
        public IActionResult Details(string url)
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    var viewName = statusCode.ToString();
                    return View("NotFound");
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}