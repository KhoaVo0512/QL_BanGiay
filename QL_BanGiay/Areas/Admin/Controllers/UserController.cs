using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Areas.Admin.Repository;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using Windows.System;
using static QL_BanGiay.Helps.RenderRazorView;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [Authorize(Roles = "Admin, Emloyee")]
    public class UserController : Controller
    {
        private readonly IUser _UserRepo;
        private readonly IRole _RoleRepo;
        private readonly IToastNotification _toastNotification;
        public UserController (IUser userRepo, IRole roleRepo, IToastNotification toastNotification)
        {
            _UserRepo = userRepo;
            _RoleRepo = roleRepo;
            _toastNotification = toastNotification;
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
        [NoDirectAccess]
        public IActionResult Details(string id)
        {
            var user = _UserRepo.GetUserDetails(id);
            return View(user);
        }
        [Route("user/edit")]
        [HttpGet]
        [NoDirectAccess]
        public IActionResult Edit(string id)
        {
            var role = _UserRepo.UserRole(id);
            var model = new List<ManageUserRolesViewModel>();
            foreach (var item in GetRole())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = item.Value,
                    RoleName = item.Text
                };
                if ( _RoleRepo.IsInRole(item.Value, role.UserName))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            role.manageUserRoles = model;
            return View(role);
        }
        [HttpPost]
        [Route("user/edit")]
        public JsonResult Edit(UserRoleModel model, string id)
        {
            bool blt = false;
            try
            {
                blt = _RoleRepo.UpdateRepo(id, model);
                if (blt)
                {
                    Sort();
                    var items = _UserRepo.GetItems("NameUsername", SortOrder.Ascending, "", 1, 5);
                    var pager = new PagerModel(items.TotalRecords, 1, 5);
                    pager.SortExpression = "";
                    this.ViewBag.Pager = pager;
                    TempData["CurrentPage"] = 1;
                    _toastNotification.AddSuccessToastMessage("Cập nhật quyền người dùng thành công");
                    return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", items, pager, "") });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Lỗi cập nhật quyền người dùng");
                    return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
                }
            }catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage("Lỗi cập nhật quyền người dùng");
                return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
            }
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
            return lstRole;
        }
        private void Sort()
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("NameUsername");
            sortModel.AddColumn("IdUser");
            sortModel.AddColumn("NameUser");
            sortModel.AddColumn("Date");
            sortModel.ApplySort("");
            ViewData["sortModel"] = sortModel;
            ViewBag.SearchText = "";
        }
    }
}
