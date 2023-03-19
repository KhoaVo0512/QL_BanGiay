using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
    public class ShoeDetailsController : Controller
    {
        private readonly IShoe _ShoeRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IShoeDetails _ShoeDetailsRepo;
        private readonly IToastNotification _toastNotification;
        public ShoeDetailsController(IToastNotification toastNotification,IShoe shoe, IWebHostEnvironment webHostEnvironment, IShoeDetails shoeDetails) 
        {
            _ShoeRepo = shoe;
            _webHostEnvironment = webHostEnvironment;
            _ShoeDetailsRepo = shoeDetails;
            _toastNotification = toastNotification;
        
        }
        [Route("shoedetails")]
        [Route("shoedetails/index")]
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
        [Route("shoedetails/edit")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Edit(string id)
        {
            ShoeDetails item = _ShoeDetailsRepo.GetItem(id);
            return View(item);
        }
        [Route("shoedetails/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShoeDetails shoe)
        {
            bool bln = false;
            if (ModelState.IsValid)
            {
                try
                {
                    bln = _ShoeDetailsRepo.Edit(shoe);
                    if (bln)
                    {
                        Sort();
                        var items = _ShoeRepo.GetItems("NameShoe", SortOrder.Ascending, "", 1, 5);
                        var pager = new PagerModel(items.TotalRecords, 1, 5);
                        pager.SortExpression = "";
                        this.ViewBag.Pager = pager;
                        TempData["CurrentPage"] = 1;
                        _toastNotification.AddSuccessToastMessage("Sản phẩm được sửa nội dung thành công thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", shoe, null, "") });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.ToString());
                    _toastNotification.AddErrorToastMessage("Lỗi nhập sản phẩm");
                    return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "create", shoe, null, "") });
                }
            }
            return View();
        }
        [Route("shoedetails/UploadImage")]
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
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("MaGiay");
            sortModel.AddColumn("NameShoe");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}
