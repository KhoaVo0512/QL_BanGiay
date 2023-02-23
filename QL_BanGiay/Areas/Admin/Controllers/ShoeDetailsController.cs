using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using System.Drawing.Printing;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class ShoeDetailsController : Controller
    {
        private readonly IShoe _ShoeRepo;
        public ShoeDetailsController (IShoe shoe)
        {
            _ShoeRepo = shoe;
        }

        [Route("shoedetails")]
        [Route("shoedetails/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("MaGiay");
            sortModel.AddColumn("NameShoe");
            sortModel.AddColumn("Price");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Giay> items = _ShoeRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("shoedetails/edit")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Edit(string id)
        {
            ShoeContext item = _ShoeRepo.GetItem(id);
            return View(item);
        }
    }
}
