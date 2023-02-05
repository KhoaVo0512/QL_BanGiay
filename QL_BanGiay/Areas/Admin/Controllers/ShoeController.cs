using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;

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
        public ShoeController (IShoe shoe, ICollection collection, IBrand brandRepo, ISupplier supplier, IProduce produce)
        {
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
        public IActionResult Create()
        {
            ViewBag.BrandList = GetBrands();
            ViewBag.ProduceList = GetProduce();
            ViewBag.CollectionList = GetCollections();
            return View(new ShoeContext());
        }
        private List<SelectListItem> GetCollections()
        {
            var lstCollection = new List<SelectListItem>();
            PaginatedList<DongSanPham> collections = _CollectionRepo.GetItems("NameCollection", SortOrder.Ascending, "",1, 1000);
            lstCollection = collections.Select(ut => new SelectListItem()
            {
                Value = ut.MaDongSanPham.ToString(),
                Text = ut.TenDongSanPham
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn dòng sản phẩm----"
            };
            lstCollection.Insert(0, defItem);
            return lstCollection;
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
                Text = "----Chọn nhãn hiệu----"
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
        
    }
}
