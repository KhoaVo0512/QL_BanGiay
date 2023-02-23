using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Models;
using System.Data;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class PurchaseOrderController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IPurchaseOrder _PurchaseOrderRepo;
        private readonly IShoe _ShoeRepo;
        private readonly ISize _SizeRepo;
        private readonly ISupplier _SupplierRepo;
        public PurchaseOrderController(IToastNotification nToastNotify, IShoe shoeRepo, IPurchaseOrder purchase, ISize sizeRepo, ISupplier supplierRepo)
        {
            _toastNotification = nToastNotify;
            _ShoeRepo = shoeRepo;
            _PurchaseOrderRepo = purchase;
            _SizeRepo = sizeRepo;
            _SupplierRepo = supplierRepo;
        }
        [Route("purchaseorder")]
        [Route("purchaseorder/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("SuppliersId");
            sortModel.AddColumn("OrderId");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<NhapHang> items = _PurchaseOrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("purchaseorder/create")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Create()
        {
            NhapHang item = new NhapHang();
            item.NhapHangCts.Add(new NhapHangCt() { MaNhapHangCt = 1 });
            ViewBag.ProductList = GetProducts();
            ViewBag.SizeList = GetSizes();
            ViewBag.SupplierList = GetSuppliers();
            item.MaNhapHang = _PurchaseOrderRepo.GetNewPONumber();
            return View(item);
        }
        [Route("purchaseorder/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(NhapHang nhaphang)
        {
            nhaphang.NhapHangCts.RemoveAll(a => a.SoLuong == 0);
            nhaphang.MaNhapHang = 0;
            if (ModelState.IsValid)
            {
                var item = nhaphang.NhapHangCts.ToArray();

                for (int i = 0; i < nhaphang.NhapHangCts.Count; i++)
                {
                    for (int j = 1; j < nhaphang.NhapHangCts.Count; j++)
                    {
                        if (item[i].MaGiay == item[j].MaGiay && item[i].MaSize == item[j].MaSize)
                        {
                            ModelState.AddModelError("NhapHangCts["+j+"].MaGiay", "Errors occurred in the Model");
                            ViewBag.SupplierList = GetSuppliers();
                            ViewBag.ProductList = GetProducts();
                            ViewBag.SizeList = GetSizes();
                            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", nhaphang, null, "") });
                        }
                    }
                }
                try
                {
                    nhaphang = await _PurchaseOrderRepo.Create(nhaphang);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
                Sort();
                var items = _PurchaseOrderRepo.GetItems("SuppliersId", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Hóa đơn nhập hàng thêm thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            ViewBag.SupplierList = GetSuppliers();
            ViewBag.ProductList = GetProducts();
            ViewBag.SizeList = GetSizes();
            _toastNotification.AddErrorToastMessage("Lỗi nhập hóa đơn nhập hàng");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", nhaphang, null, "") });
        }
        [Route("purchaseorder/details")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Details(int id)
        {
            NhapHang item = _PurchaseOrderRepo.GetItem(id);
            ViewBag.SupplierList = GetSuppliers();
            ViewBag.ProductList = GetProducts();
            ViewBag.SizeList = GetSizes();
            return View(item);
        }
        [Route("purchaseorder/delete")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var nhapHang = _PurchaseOrderRepo.GetItem(id);
            if (nhapHang == null)
            {
                return NotFound();
            }

            return View(nhapHang);
        }
        [Route("purchaseorder/deleteConfirm")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirm(int id)
        {

            bool bolret = false;
            string errMessage = "";
            try
            {
                bolret = _PurchaseOrderRepo.Delete(id);
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                errMessage = errMessage + " " + _PurchaseOrderRepo.GetErrors();
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
            }
            Sort();
            var items = _PurchaseOrderRepo.GetItems("SuppliersId", SortOrder.Ascending, "", 1, 5);
            var pager = new PagerModel(items.TotalRecords, 1, 5);
            pager.SortExpression = "";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = 1;
            _toastNotification.AddSuccessToastMessage("Hóa đơn nhập hàng đã xóa thành công");
            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
        }
        [Route("purchaseorder/edit")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Edit(int id)
        {
            NhapHang item = _PurchaseOrderRepo.GetItem(id);
            ViewBag.ProductList = GetProducts();
            ViewBag.SizeList = GetSizes();
            ViewBag.SupplierList = GetSuppliers();
            return View(item);
        }
        [Route("purchaseorder/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(NhapHang item)
        {
            item.NhapHangCts.RemoveAll(a => a.SoLuong == 0);
            bool bolret = false;
            if (ModelState.IsValid)
            {

                try
                {
                    bolret = _PurchaseOrderRepo.Edit(item);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                }
                Sort();
                var items = _PurchaseOrderRepo.GetItems("SuppliersId", SortOrder.Ascending, "", 1, 5);
                var pager = new PagerModel(items.TotalRecords, 1, 5);
                pager.SortExpression = "";
                this.ViewBag.Pager = pager;
                TempData["CurrentPage"] = 1;
                _toastNotification.AddSuccessToastMessage("Hóa đơn nhập hàng được sửa thành công");
                return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
            }
            ViewBag.SupplierList = GetSuppliers();
            ViewBag.ProductList = GetProducts();
            ViewBag.SizeList = GetSizes();
            _toastNotification.AddErrorToastMessage("Lỗi sửa hóa đơn nhập hàng");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", item, null, "") });
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
        private List<SelectListItem> GetSuppliers()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<DonViNhapHang> products = _SupplierRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.MaDonViNhap.ToString(),
                Text = ut.TenDonViNhap
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn nhà cung cấp----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
        private List<SelectListItem> GetProducts()
        {
            var lstProducts = new List<SelectListItem>();

            PaginatedList<Giay> products = _ShoeRepo.GetItems("Name", SortOrder.Ascending, "", 1, 1000);

            lstProducts = products.Select(ut => new SelectListItem()
            {
                Value = ut.MaGiay.ToString(),
                Text = ut.TenGiay
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn sản phẩm----"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Id");
            sortModel.AddColumn("SuppliersId");
            sortModel.AddColumn("OrderId");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}