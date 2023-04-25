using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Controllers
{
    [Route("vans")]
    public class VansController : Controller
    {
        private readonly ICollection _CollectionRepo;
        private readonly IVans _VansRepo;

        public VansController(ICollection collectionRepo, IVans vans)
        {
            _CollectionRepo = collectionRepo;
            _VansRepo = vans;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 12)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameShoe");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Giay> items = _VansRepo.GetItemsVans(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        [Route("{namecollection}")]
        public IActionResult Collection(string namecollection, string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 12)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameShoe");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Giay> items = _VansRepo.GetItemsVansCollection(namecollection, sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.Action = namecollection;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        [Route("GetCollection")]
        public JsonResult GetCollection()
        {
            var items = _CollectionRepo.GetCollectionVans();
            return new JsonResult(items);
        }
    }
}
