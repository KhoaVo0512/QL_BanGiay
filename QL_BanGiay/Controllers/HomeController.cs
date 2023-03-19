using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Models;
using System.Diagnostics;

namespace QL_BanGiay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoe _ShoeRepo;
        private readonly ISize _SizeRepo;

        public HomeController(ILogger<HomeController> logger, IShoe shoe, ISize sizeRepo)
        {
            _logger = logger;
            _ShoeRepo = shoe;
            _SizeRepo = sizeRepo;
        }

        public IActionResult Index()
        {
            ViewBag.ItemsVans = _ShoeRepo.GetItemsVans();
            ViewBag.ItemsConverse = _ShoeRepo.GetItemsConverse();
            ViewBag.ItemsAdidas = _ShoeRepo.GetItemsAdidas();
            ViewBag.ItemsNike = _ShoeRepo.GetItemsNike();
            return View();
        }

        [Route("/{url}")]
        [HttpGet]
        public IActionResult Details(string url)
        {
            bool checkShoe = _ShoeRepo.IsNameShoeNoExists(url);
            if (checkShoe)
            {
                var item = _ShoeRepo.GetItemProductDetails(url);
                ViewBag.SizeList = GetSizes();
                return View(item);
            }
            else
                return View("NotFound");
        }
        [Route("cart")]
        [HttpGet]
        public IActionResult Card()
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
        private List<SelectListItem> GetSizes()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<SizeGiay> products = _SizeRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.MaSize.ToString(),
                Text = ut.TenSize
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "VUI LÒNG CHỌN SIZE"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
    }
}