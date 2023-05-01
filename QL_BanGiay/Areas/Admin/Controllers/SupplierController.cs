using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Authorize(Roles = "Admin, Emloyee")]
    public class SupplierController : Controller
    {
        private readonly ISupplier _SuppilerRepo;
        private readonly IToastNotification _toastNotification;
        public SupplierController(ISupplier supplier, IToastNotification toastNotification)
        {
            _SuppilerRepo = supplier;
            _toastNotification = toastNotification;
        }
        [Route("supplier")]
        [Route("supplier/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameSupplier");
            sortModel.AddColumn("Email");
            sortModel.AddColumn("Address");
            sortModel.AddColumn("Phone");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;
            PaginatedList<DonViNhapHang> items = _SuppilerRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [HttpGet]
        [Route("supplier/create")]
        [NoDirectAccess]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("supplier/create")]
        public JsonResult Create(DonViNhapHang model)
        {
            bool blt = false;
            if (ModelState.IsValid)
            {
                try
                {
                    blt = _SuppilerRepo.EditSupplier(model);
                    if (blt)
                    {
                        Sort();
                        var items = _SuppilerRepo.GetItems("NameSupplier", SortOrder.Ascending, "", 1, 5);
                        var pager = new PagerModel(items.TotalRecords, 1, 5);
                        pager.SortExpression = "";
                        this.ViewBag.Pager = pager;
                        TempData["CurrentPage"] = 1;
                        _toastNotification.AddSuccessToastMessage("Đơn vị nhập hàng thêm thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
                    }else
                    {
                        _toastNotification.AddErrorToastMessage("Lỗi SQL server");      
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
            }
            _toastNotification.AddErrorToastMessage("Lỗi nhập đơn vị nhập hàng");
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
        }
        [HttpGet]
        [Route("supplier/edit")]
        [NoDirectAccess]
        public IActionResult Edit(int id)
        {
            DonViNhapHang item = _SuppilerRepo.GetItem(id);
            return View(item);
        }
        [HttpPost]
        [Route("supplier/edit")]
        public JsonResult Edit(DonViNhapHang model)
        {
            bool blt = false;
            if (ModelState.IsValid)
            {
                try
                {
                    blt = _SuppilerRepo.EditSupplier(model);
                    if (blt)
                    {
                        Sort();
                        var items = _SuppilerRepo.GetItems("NameSupplier", SortOrder.Ascending, "", 1, 5);
                        var pager = new PagerModel(items.TotalRecords, 1, 5);
                        pager.SortExpression = "";
                        this.ViewBag.Pager = pager;
                        TempData["CurrentPage"] = 1;
                        _toastNotification.AddSuccessToastMessage("Chỉnh sửa đơn vị nhập hàng thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Lỗi SQL server");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", model, null, "") });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
            }
            _toastNotification.AddErrorToastMessage("Lỗi nhập đơn vị nhập hàng");
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", model, null, "") });
        }
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameSupplier");
            sortModel.AddColumn("Email");
            sortModel.AddColumn("Address");
            sortModel.AddColumn("Phone");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}
