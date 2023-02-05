using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Models;
using System.Diagnostics;

namespace QL_BanGiay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
        //public IActionResult Error(int statuscode)
        //{
        //    if (statuscode == 404)
        //    {
        //        return View("NotFound");
        //    }else
        //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}