using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using System.Drawing.Printing;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class CheckOutController : Controller
    {
        private readonly IBill _BillRepo;
        private readonly IAddress _AddressRepo;
        private readonly IToastNotification _toastNotification;
        private readonly ISize _SizeRepo;
        public CheckOutController(IBill BillRepo, IAddress addressRepo, IToastNotification toastNotification, ISize sizeRepo)
        {
            _BillRepo = BillRepo;
            _AddressRepo = addressRepo;
            _toastNotification = toastNotification;
            _SizeRepo = sizeRepo;
        }
        [Route("checkout")]
        [Route("checkout/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Price");
            sortModel.AddColumn("NgayGiaoDH");
            sortModel.AddColumn("ngaylapHD");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<HoaDon> items = _BillRepo.GetItems("", sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("checkout/trangthai")]
        public JsonResult TrangThai(string mahd,string id)
        {
            bool isValid = _BillRepo.ChangeBill(mahd, id);
            _toastNotification.AddSuccessToastMessage("Trạng thái hóa đơn thay đổi thành công");
            return Json(isValid);
        }
        [Route("checkout/Details")]
        [HttpGet]
        public IActionResult Details(string id)
        {
            List<SelectListItem> listChoose = new List<SelectListItem>();
            listChoose.Add(new SelectListItem() { Text = "Đã xác nhận đơn hàng", Value = "0" });
            listChoose.Add(new SelectListItem() { Text = "Đang vận chuyển", Value = "1" });
            listChoose.Add(new SelectListItem() { Text = "Đã giao", Value = "2" });
            ViewBag.TrangThai = listChoose;
            ViewBag.SizeList = GetSizes();
            HoaDon item = _BillRepo.GetItem(id);
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
