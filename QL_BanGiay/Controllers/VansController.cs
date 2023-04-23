using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;

namespace QL_BanGiay.Controllers
{
    public class VansController : Controller
    {
        private readonly ICollection _CollectionRepo;

        public VansController(ICollection collectionRepo)
        {
            _CollectionRepo = collectionRepo;
        }
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            //SortModel sortModel = new SortModel();
            //sortModel.ApplySort(sortExpression);
            //ViewData["sortModel"] = sortModel;
            //ViewBag.SearchText = SearchText;
            //PaginatedList<Giay> items = _ShoeRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            //var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            //pager.Action = "Search";
            //this.ViewBag.Pager = pager;
            //TempData["CurrentPage"] = pg;
            //return View(items);
            return View();
        }
        [HttpGet]
        public JsonResult GetCollection()
        {
            var items = _CollectionRepo.GetCollectionVans();
            return new JsonResult(items);
        }
    }
}
