using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    //[Route("admin")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            ViewData["location"] = location;
            return View();
        }
    }
    
}
