using elFinder.NetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using Uni_Shop.Service;

namespace QL_BanGiay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShoe _ShoeRepo;
        private readonly ISize _SizeRepo;
        private readonly IAccount _AccountRepo;
        private readonly IUser _UserRepo;
        private readonly ICheckout _CheckoutRepo;
        private readonly IToastNotification _toastNotification;
        private readonly IAddress _AddressRepo;
        private readonly IOrder _OrderRepo;
        private static CheckOutModel? _model;
        private static CheckOutUserModel? _modelUser;
        private string _note;

        public HomeController(ILogger<HomeController> logger, IShoe shoe, ISize sizeRepo, IAccount account, 
            ICheckout checkoutRepo, IToastNotification toastNotification, IAddress addressRepo, IUser userRepo
            ,IOrder order)
        {
            _logger = logger;
            _ShoeRepo = shoe;
            _SizeRepo = sizeRepo;
            _AccountRepo = account;
            _CheckoutRepo = checkoutRepo;
            _toastNotification = toastNotification;
            _AddressRepo = addressRepo;
            _UserRepo = userRepo;
            _OrderRepo = order;
        }

        public IActionResult Index()
        {
            ViewBag.ItemsVans = _ShoeRepo.GetItemsVans();
            ViewBag.ItemsConverse = _ShoeRepo.GetItemsConverse();
            ViewBag.ItemsAdidas = _ShoeRepo.GetItemsAdidas();
            ViewBag.ItemsNike = _ShoeRepo.GetItemsNike();
            return View();
        }

        [Route("/{url}")]
        [HttpGet]
        public IActionResult Details(string url)
        {
            bool checkShoe = _ShoeRepo.IsNameShoeNoExists(url);
            if (checkShoe)
            {
                var item = _ShoeRepo.GetItemProductDetails(url);
                ViewBag.SizeList = GetSizes(item.MaGiay);
                return View(item);
            }
            else
                return View("NotFound");
        }
        [Route("cart")]
        [HttpGet]
        public IActionResult Cart()
        {
            return View();
        }
        [Route("/home/cartdetails")]
        [HttpGet]
        public IActionResult CartDetails(string idSanPham, int SoLuong, string SizeGiay, int idSize) 
        {
            var item = _ShoeRepo.GetItemCart(idSanPham);
            ViewBag.SoLuong = SoLuong;
            ViewBag.SizeGiay = SizeGiay;
            ViewBag.IdSize = idSize;
            return View(item);
        }
        [Route("checkout")]
        [HttpGet]
        public IActionResult Checkout()
        {
            var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                            .Select(c => c.Value).SingleOrDefault();

            if (sId != null)
            {
                var item = _UserRepo.GetUser(sId);
                return View(item);
            }
            return View();
        }
        [Route("home/checkproduct")]
        [HttpGet]
        public async Task<int> CheckProduct(string id, string idSize)
        {
            if (idSize == "0")
            {
                return 0;
            }
            int quantity = await _CheckoutRepo.CheckProduct(id, idSize);
            return quantity;
        }
        [Route("/home/checkoutdetails")]
        [HttpGet]
        public IActionResult CheckoutDetails(string idSanPham, int SoLuong, string SizeGiay, int idSize)
        {
            var item = _ShoeRepo.GetItemCart(idSanPham);
            ViewBag.SoLuong = SoLuong;
            ViewBag.SizeGiay = SizeGiay;
            ViewBag.IdSize = idSize;
            return View(item);
        }
        [Route("/home/checkoutdetails/{note}")]
        [HttpPost]
        public async Task<JsonResult> CheckoutDetails(CheckOutModel model, string? note)
        {
            string bln = await _AccountRepo.NguoiDung(model, note);
            _model = model;
            HttpContext.Session.SetString("IdNguoiDung", bln);
            return Json(new { isValid = true, id = bln });
        }
        [Route("/home/getjsondata/{id}/{vnpay}")]
        [HttpPost]
        public JsonResult GetJsonData([FromBody] ProductModel[] Stock, string id, string vnpay)
        {
            DonDat item = _CheckoutRepo.CreateCheckout(Stock, id);
            if (item != null)
            {
                
                if (vnpay == "0")
                {
                    TempData["Message"] = "Đơn hàng của bạn đã đặt thành công. Vui lòng kiểm tra mail để biết thêm chi tiết.";
                    HttpContext.Session.SetString("IdDonDat", item.MaDonDat);
                    var sendMail = _OrderRepo.GetMail(item.MaDonDat);
                    SendMail(sendMail);
                    return Json("Success");
                }else
                {
                    HttpContext.Session.SetString("IdDonDat", item.MaDonDat);
                    return Json(new {total = item.Total});
                }
            }
            else
                return Json("Faild");
        }
        [Route("/home/checkoutuser")]
        [HttpPost]
        public JsonResult CheckoutUser(CheckOutUserModel model)
        {
            _modelUser = model;
            return Json("True");
        }
        [Route("/home/test/{id}/{vnpay}/{note}")]
        [HttpPost]
        public JsonResult Test([FromBody] ProductModel[] Stock, string id, string vnpay, string note)
        {
            var item = _CheckoutRepo.CreateCheckoutUser(_modelUser, Stock, id, vnpay, note);
            if (item != null)
            {
                if (vnpay == "0")
                {
                    TempData["Message"] = "Đơn hàng của bạn đã đặt thành công. Vui lòng kiểm tra mail để biết thêm chi tiết.";
                    HttpContext.Session.SetString("IdDonDat", item.MaDonDat);
                    var sendMail = _OrderRepo.GetMail(item.MaDonDat);
                    SendMail(sendMail);
                    return Json("Success");
                }
                else
                {
                    HttpContext.Session.SetString("IdDonDat", item.MaDonDat);
                    return Json(new { total = item.Total });
                }
            }
            else
                return Json("Faild");
        }

        [Route("/Home/Payment/{note}")]
        public async Task<JsonResult> PaymentAsync(int? note)
        {
            string url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            string returnUrl = "https://" + HttpContext.Request.Host.Value.ToLower() + "/Home/PaymentConfirm";
            string tmnCode = "L6U7UZK9";
            string hashSecret = "POGMJYCCKYJCPIGWRZNMEMBAAEXOQDYL";
            PaymentServices pay = new PaymentServices();
            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_Amount", (note * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_IpAddr", PaymentServices.GetIpAddress(HttpContext)); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            return Json(new { isValid = true, action = paymentUrl });
        }
        public async Task<IActionResult> PaymentConfirm()
        {
            if (Request.QueryString.Value.Count() != 0)
            {
                string hashSecret = "POGMJYCCKYJCPIGWRZNMEMBAAEXOQDYL";//Chuỗi bí mật
                NameValueCollection vnpayData = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                PaymentServices pay = new PaymentServices();
                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }
                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.Query["vnp_SecureHash"]; //hash của dữ liệu trả về
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?
                var idMaDonDat = HttpContext.Session.GetString("IdDonDat");
                var idNguoiDung = HttpContext.Session.GetString("IdNguoiDung");
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {

                        bool check = _CheckoutRepo.EditCheckOut(idMaDonDat, orderId);
                        var item = _OrderRepo.GetMail(idMaDonDat);
                        SendMail(item);
                        //Thanh toán thành công
                        TempData["Message"] = "Thanh toán thành công hóa đơn vui lòng kiểm tra mail để biết thêm chi tiết";
                        ViewBag.vnpayTranId = vnpayTranId;
                        TempData["ClearStogare"] = "Success";
                        return Redirect("/");
                    }
                    else
                    {
                        bool remove = await _CheckoutRepo.DeleteNguoiDungAndDonDat(idMaDonDat, idNguoiDung);
                        TempData["Message"] = "Có lỗi xảy ra trong quá trình xử lý hóa đơn vui lòng thử lại";
                        return View("Checkout", _model);
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return Redirect("/checkout");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    var viewName = statusCode.ToString();
                    return View("NotFound");
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<SelectListItem> GetSizes(string id)
        {
            var lstProducts = new List<SelectListItem>();
            List<KhoGiay> items = _SizeRepo.GetSizeShoe(id);
            lstProducts = items.Select(ut => new SelectListItem()
            {
                Value = ut.MaSize.ToString(),
                Text = ut.MaSizeNavigation.TenSize
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "VUI LÒNG CHỌN SIZE"
            };

            lstProducts.Insert(0, defItem);

            return lstProducts;
        }
        [Route("search")]
        [HttpGet]
        public IActionResult Search(string sortExpression = "",string SearchText="", int pg = 1, int pageSize = 10)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameShoe");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = SearchText;
            PaginatedList<Giay> items = _ShoeRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel( items.TotalRecords,pg, pageSize);
            pager.Action = "Search";
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        private void SendMail(EmailModel model)
        {
            CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
           
            string FilePath = Directory.GetCurrentDirectory() + "\\template\\send2.html";
            using (StreamReader SourceReader = System.IO.File.OpenText(FilePath))
            {
                string contentCustomer = SourceReader.ReadToEnd();
                var strSanPham = "";
                int thanhtien = 0;
                var tongtien = decimal.Zero;
                foreach (var item in model.DonDatCts)
                {
                    var total = String.Format(elGR, "{0:0,0}", (item.DonGia * item.SoLuong)) + " VND";
                    strSanPham += "<tr>";
                    strSanPham += "<td>" + item.MaGiayNavigation.TenGiay + "</td>";
                    strSanPham += "<td>" + item.SoLuong + "</td>";
                    strSanPham += "<td>" + total + "</td>";
                    strSanPham += "</tr>";
                    thanhtien += Convert.ToInt32(item.DonGia) * Convert.ToInt32(item.SoLuong);
                }
                tongtien = thanhtien;
                contentCustomer = contentCustomer.Replace("{{MaDon}}", model.MaHoaDon);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", model.HoTen);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", model.NgayDat.ToString("dd/MM/yyyy hh:mm:ss tt"));
                contentCustomer = contentCustomer.Replace("{{Phone}}", model.Sdt);
                contentCustomer = contentCustomer.Replace("{{Email}}", model.Email);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", model.DiaChi);
                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", String.Format(elGR, "{0:0,0}",thanhtien));
                contentCustomer = contentCustomer.Replace("{{TongTien}}", String.Format(elGR, "{0:0,0}", tongtien));
                bool btl = SendMails.SendMail("BanGiay", "Đơn hàng #" + model.MaHoaDon, contentCustomer, model.Email);
            }

        }
        
    }
}