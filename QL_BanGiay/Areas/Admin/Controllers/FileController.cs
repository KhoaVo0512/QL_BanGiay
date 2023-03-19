using Microsoft.AspNetCore.Mvc;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
