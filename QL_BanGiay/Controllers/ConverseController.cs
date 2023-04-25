using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;
using QL_BanGiay.Repository;

namespace QL_BanGiay.Controllers
{
    [Route("converse")]
    public class ConverseController : Controller
    {
        private readonly ICollection _CollectionRepo;
        private readonly IConverse _ConverseRepo;
        public ConverseController (ICollection collectionRepo, IConverse converse)
        {
            _CollectionRepo = collectionRepo;
            _ConverseRepo = converse;
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
            PaginatedList<Giay> items = _ConverseRepo.GetItemsConverse(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        [Route("GetCollection")]
        public JsonResult GetCollection()
        {
            var items = _CollectionRepo.GetCollectionConverse();
            return new JsonResult(items);
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
            PaginatedList<Giay> items = _ConverseRepo.GetItemsConverseCollection(namecollection, sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.Action = namecollection;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
    }
}
