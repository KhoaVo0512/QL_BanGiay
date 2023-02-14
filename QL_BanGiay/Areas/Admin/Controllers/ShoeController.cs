using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class ShoeController : Controller
    {
        private readonly IShoe _ShoeRepo;
        private readonly ICollection _CollectionRepo;
        private readonly IBrand _BrandRepo;
        private readonly ISupplier _SupplierRepo;
        private readonly IProduce _ProduceRepo;
        private readonly IToastNotification _toastNotification;
        public ShoeController (IShoe shoe, ICollection collection, IBrand brandRepo, ISupplier supplier, IProduce produce, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _ProduceRepo = produce;
            _SupplierRepo = supplier;
            _CollectionRepo = collection;
            _ShoeRepo = shoe;            
            _BrandRepo = brandRepo;

        }
        [Route("shoe")]
        [Route("shoe/index")]
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
                    _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
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
                var items = _ShoeRepo.GetItems("NameShoe", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Sản phẩm đã được thêm thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", item, null, "") });
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
            sortModel.AddColumn("MaGiay");
            sortModel.AddColumn("NameShoe");
            sortModel.AddColumn("Price");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }

    }
}
