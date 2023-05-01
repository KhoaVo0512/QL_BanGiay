using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using System.Data;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Authorize(Roles = "Admin, Emloyee")]
    public class ShoeController : Controller
    {
        private readonly IShoe _ShoeRepo;
        private readonly ICollection _CollectionRepo;
        private readonly IBrand _BrandRepo;
        private readonly ISupplier _SupplierRepo;
        private readonly IProduce _ProduceRepo;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ShoeController (IShoe shoe, ICollection collection, IBrand brandRepo, ISupplier supplier
            , IProduce produce, IToastNotification toastNotification, IWebHostEnvironment webHostEnvironment)
        {
            _toastNotification = toastNotification;
            _ProduceRepo = produce;
            _SupplierRepo = supplier;
            _CollectionRepo = collection;
            _ShoeRepo = shoe;            
            _BrandRepo = brandRepo;
            _webHostEnvironment = webHostEnvironment;

        }
        [Route("shoe")]
        [Route("shoe/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Date");
            sortModel.AddColumn("IdShoe");
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
        [Route("shoe/create")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Create()
        {
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            return View(new ShoeContext());
        }
        [Route("shoe/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateAsync(ShoeContext item)
        {
            bool CheckMaGiay = false;
            if (ModelState.IsValid)
            {
                CheckMaGiay = _ShoeRepo.IsShoeNoExists(item.MaGiay);
                if (CheckMaGiay)
                {
                    ModelState.AddModelError("MaGiay", "Mã giày này đã có rồi");
                    _toastNotification.AddErrorToastMessage("Mã sản phẩm nầy có rồi");
                    ViewBag.BrandList = GetBrands();
                    ViewBag.ProduceList = GetProduce();
                    return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", item, null, "") });
                }
                try
                {
                    item = await _ShoeRepo.Create(item);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
                Sort();
                var items = _ShoeRepo.GetItems("Date", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;

                _toastNotification.AddSuccessToastMessage("Sản phẩm đã được thêm thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", item, null, "") });
        }
        [Route("shoe/delete")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Delete(string id)
        {
            if (id == null)
                return NotFound();
            var shoe = _ShoeRepo.GetItem(id);
            if (shoe == null)
            {
                return NotFound();
            }
            return View(shoe);
        }
        [Route("shoe/delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComfirm(string id)
        {
            var shoe = _ShoeRepo.Delete(id);
            Sort();
            var items = _ShoeRepo.GetItems("Date", SortOrder.Ascending, "", 1, 5);
            var pager = new PagerModel(items.TotalRecords, 1, 5);
            pager.SortExpression = "";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = 1;
            _toastNotification.AddSuccessToastMessage("Sản phẩm đã xóa thành công");
            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
        }
        [Route("shoe/detail")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Detail(string id)
        {
            EditShoeModel item = _ShoeRepo.GetItem(id);
            var collection = _CollectionRepo.GetItem(item.MaDongSanPham);
            ViewBag.NameCollection = collection.TenDongSanPham;
            ViewBag.IdCollection = collection.MaDongSanPham;
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            return View(item);
        }

        [Route("shoe/edit")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Edit(string id)
        {
            EditShoeModel item = _ShoeRepo.GetItem(id);
            var collection = _CollectionRepo.GetItem(item.MaDongSanPham);
            ViewBag.NameCollection = collection.TenDongSanPham;
            ViewBag.IdCollection = collection.MaDongSanPham;
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            return View(item);
        }
        [Route("shoe/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(EditShoeModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var getShoe = await _ShoeRepo.Edit(item);
                }catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                    ViewBag.BrandList = GetBrands();
                    ViewBag.ProduceList = GetProduce();
                    return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "Edit", item, null, "") });
                }
                Sort();
                var items = _ShoeRepo.GetItems("Date", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Sản phẩm được sửa thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            var collection = _CollectionRepo.GetItem(item.MaDongSanPham);
            ViewBag.NameCollection = collection.TenDongSanPham;
            ViewBag.IdCollection = collection.MaDongSanPham;
            
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", item, null, "") });
        }
        [Route("shoe/information")]
        [HttpGet]

        public IActionResult Information(string id)
        {
            var item = _ShoeRepo.GetItemInformation(id);
            return View(item);
        }
        [Route("shoe/information")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Information(ShoeDetails model)
        {
            bool bln = false;
            if (ModelState.IsValid)
            {
                try
                {
                    bln = _ShoeRepo.EditInformation(model);
                    if (bln)
                    {
                        Sort();
                        var items = _ShoeRepo.GetItems("Date", SortOrder.Ascending, "", 1, 5);
                        var pager = new PagerModel(items.TotalRecords, 1, 5);
                        pager.SortExpression = "";
                        this.ViewBag.Pager = pager;
                        TempData["CurrentPage"] = 1;
                        _toastNotification.AddSuccessToastMessage("Sản phẩm được sửa nội dung thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "information", model, null, "") });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                    _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
                    return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "information", model, null, "") });
                }
            }
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "information", model, null, "") });
        }
        [Route("shoe/GetCollections")]
        public JsonResult GetCollections(int id)
        {
            var lstCollection = _CollectionRepo.GetCollections(id);
            return new JsonResult(lstCollection);
        }
        private List<SelectListItem> GetBrands() 
        {
            var lstBrand = new List<SelectListItem>();
            PaginatedList<NhanHieu> brands = _BrandRepo.GetItems("NameBrand", SortOrder.Ascending,"", 1, 1000);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaNhanHieu.ToString(),
                Text = ut.TenNhanHieu
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn nhãn hiệu----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        private List<SelectListItem> GetProduce()
        {
            var lstProduce = new List<SelectListItem>();
            PaginatedList<NoiSanXuat> produce = _ProduceRepo.GetItems("nameproduce", SortOrder.Ascending, "", 1, 1000);
            lstProduce = produce.Select(ut => new SelectListItem()
            {
                Value = ut.MaNhaSanXuat.ToString(),
                Text = ut.TenNhaSanXuat
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn nhà sản xuất----"
            };
            lstProduce.Insert(0, defItem);
            return lstProduce;
        }
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Date");
            sortModel.AddColumn("NameShoe");
            sortModel.AddColumn("Price");
            sortModel.AddColumn("IdShoe");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
        [Route("shoe/UploadImage")]
        [HttpPost]
        public IActionResult UploadImage(List<IFormFile> image)
        {
            List<string> filepath = new List<string>();

            foreach (IFormFile item in Request.Form.Files)
            {
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image", item.FileName);
                using (var stream = new FileStream(serverFolder, FileMode.Create))
                {
                    item.CopyTo(stream);

                }
                filepath.Add("/Image/" + item.FileName);
            }
            return Json(new { url = filepath });
        }

    }
}
