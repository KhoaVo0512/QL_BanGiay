using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin, Emloyee")]
    public class UserController : Controller
    {
        private readonly IUser _UserRepo;
        private readonly IRole _RoleRepo;
        public UserController (IUser userRepo, IRole roleRepo)
        {
            _UserRepo = userRepo;
            _RoleRepo = roleRepo;
        }
        [Route("user")]
        [Route("user/index")]
        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameUsername");
            sortModel.AddColumn("IdUser");
            sortModel.AddColumn("NameUser");
            sortModel.AddColumn("Date");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;
            PaginatedList<TaiKhoan> items = _UserRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);
            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;
            TempData["CurrentPage"] = pg;
            return View(items);
        }
        [Route("user/details")]
        [HttpGet]
        public IActionResult Details(string id)
        {
            var user = _UserRepo.GetUserDetails(id);
            return View(user);
        }
        [Route("user/edit")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var role = _UserRepo.UserRole(id);
            ViewBag.Role = GetRole();
            return View(role);
        }
        private List<SelectListItem> GetRole()
        {
            var lstRole = new List<SelectListItem>();
            PaginatedList<Quyen> role = _RoleRepo.GetItems("namerole", SortOrder.Ascending, "", 1, 1000);
            lstRole = role.Select(ut => new SelectListItem()
            {
                Value = ut.MaQuyen.ToString(),
                Text = ut.TenQuyen
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn nhà sản xuất----"
            };
            lstRole.Insert(0, defItem);
            return lstRole;
        }
    }
}
