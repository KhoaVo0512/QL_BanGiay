﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class WareHouseController : Controller
    {
        private readonly IShoe _ShoeRepo;
        private readonly ISize _SizeRepo;
        public WareHouseController(IShoe shoeRepo, ISize sizeRepo)
        {
            _ShoeRepo = shoeRepo;
            _SizeRepo = sizeRepo;
        }

        [Route("warehouse")]
        [Route("warehouse/index")]
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
        [Route("warehouse/details")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Details(string id)
        {
            Giay item = _ShoeRepo.GetItemWareHouse(id);
            ViewBag.SizeList = GetSizes();
            return View(item);
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
                Text = "----Chọn size giày----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
    }
}