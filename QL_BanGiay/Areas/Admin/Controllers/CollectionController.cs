using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using System.Drawing.Printing;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class CollectionController : Controller
    {
        private readonly ICollection _Icollection;
        private readonly IBrand _Ibrand;
        private readonly IToastNotification _toastNotification;
        public CollectionController(ICollection collection, IBrand brand, IToastNotification toastNotification) 
        {
            _toastNotification = toastNotification;
            _Ibrand = brand;
            _Icollection = collection;
        }
        [Route("collection")]
        [Route("collection/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("NameCollection");
            sortModel.AddColumn("NameBrand");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;
            PaginatedList<CollectionModel> items = _Icollection.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("collection/create")]
        [HttpGet]
        public IActionResult Create()
        {
            DongSanPham dongsanpham = new DongSanPham();
            ViewBag.BrandList = GetBrands();
            return View(dongsanpham);
        }
        [Route("collection/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(DongSanPham dongsanpham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dongsanpham = await _Icollection.Create(dongsanpham);
                }catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
                Sort();
                var items = _Icollection.GetItems("namecollection", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Dòng sản phẩm được thêm thành công");
                return Json(new {isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "")});
            }
            ViewBag.BrandList = GetBrands();
            _toastNotification.AddErrorToastMessage("Lỗi nhập dòng sản phẩm");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", dongsanpham,null,"") });
        }
        [Route("collection/delete")]
        [HttpGet]
        public IActionResult Delete (int? id)
        {
            if (id == null)
                return NotFound();
            var collection = _Icollection.GetItem(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }
        [Route("collection/delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            try
            {
                var collection = await _Icollection.Delete(id);
            }catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.ToString());
            }
            Sort();
            var items = _Icollection.GetItems("namecollection", SortOrder.Ascending, "", 1, 5);
            var pager = new PagerModel(items.TotalRecords, 1, 5);
            pager.SortExpression = "";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = 1;
            _toastNotification.AddSuccessToastMessage("Dòng sản phẩm đã xóa thành công");
            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
        }
        [Route("collection/edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            DongSanPham item = _Icollection.GetItem(id);
            ViewBag.BrandList = GetBrands();
            return View(item);
        }
        [Route("collection/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(DongSanPham dongsanpham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dongsanpham = await _Icollection.Edit(dongsanpham);
                }catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
                Sort();
                var items = _Icollection.GetItems("namecollection", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Dòng sản phẩm được sửa thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            ViewBag.BrandList = GetBrands();
            _toastNotification.AddErrorToastMessage("Lỗi sửa dòng sản phẩm");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", dongsanpham, null, "") });
        }
        private List<SelectListItem> GetBrands()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<NhanHieu> products = _Ibrand.GetItems("namebrand", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.MaNhanHieu.ToString(),
                Text = ut.TenNhanHieu
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn nhãn hiệu----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("NameCollection");
            sortModel.AddColumn("NameBrand");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}
