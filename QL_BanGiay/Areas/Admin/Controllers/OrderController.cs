
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using System;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using static QL_BanGiay.Helps.RenderRazorView;
using Windows.Media.Protection.PlayReady;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class OrderController : Controller
    {
        private readonly IOrder _OrderRepo;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISize _SizeRepo;
        public OrderController(IOrder orderRepo, IToastNotification toastNotification, ISize sizeRepo, IWebHostEnvironment webHostEnvironment)
        {
            _OrderRepo = orderRepo;
            _toastNotification = toastNotification;
            _SizeRepo = sizeRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("order")]
        [Route("order/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Date");
            sortModel.AddColumn("IdOrder");
            sortModel.AddColumn("Name");
            sortModel.AddColumn("Phone");
                    
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<DonDat> items = _OrderRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("order/delete")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Delete(string id)
        {
            if (id == null)
                return NotFound();
            var shoe = _OrderRepo.GetItem(id);
            if (shoe == null)
            {
                return NotFound();
            }
            return View(shoe);
        }
        [Route("order/delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComfirm(string id)
        {
            var dondat = _OrderRepo.Delete(id);
            Sort();
            var items = _OrderRepo.GetItems("Date", SortOrder.Descending, "", 1, 10);
            var pager = new PagerModel(items.TotalRecords, 1, 5);
            pager.SortExpression = "";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = 1;
            _toastNotification.AddSuccessToastMessage("Đơn hàng đã hủy thành công");
            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
        }
        [Route("order/details")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Details(string id)
        {
            DonDat item = _OrderRepo.GetItem(id);
            if (item.DaThanhToan == false)
            {
                ViewBag.TrangThai = "Chưa thanh toán";
            }
            else
                ViewBag.TrangThai = "Đã thanh toán";
            if (item.MaVnpay == "0")
            {
                ViewBag.HTThanhToan = "Thanh toán khi nhận hàng";
            }
            else
                ViewBag.HTThanhToan = "Đã thanh toán qua VNPay";
            ViewBag.SizeList = GetSizes();
            return View(item);
        }
        [Route("order/details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Details(DonDat item)
        {
            var MaHD = await _OrderRepo.CreateHoaDon(item);
            Sort();
            var items = _OrderRepo.GetItems("Date", SortOrder.Ascending, "", 1, 10);
            var pager = new PagerModel(items.TotalRecords, 1, 10);
            pager.SortExpression = "";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = 1;
            _toastNotification.AddSuccessToastMessage("Đơn hàng đã giao thành công.");
            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
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
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("IdOrder");
            sortModel.AddColumn("Name");
            sortModel.AddColumn("Phone");
            sortModel.AddColumn("Date");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}
