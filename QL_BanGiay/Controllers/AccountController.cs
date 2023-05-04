using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Org.BouncyCastle.Utilities;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Security.Cryptography;
using Windows.Devices.Bluetooth;
using Windows.System;

namespace QL_BanGiay.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IAccount _accountRepository;
        private readonly IProvince _ProviceRepo;
        private readonly IDistrict _DistrictRepo;
        private readonly ICommune _CommuneRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IToastNotification toastNotification, IAccount accountRepository
            , IProvince proviceRepo, IDistrict district, ICommune commune, IWebHostEnvironment webHostEnvironment)
        {
            _DistrictRepo = district;
            _CommuneRepo = commune;
            _toastNotification = toastNotification;
            _accountRepository = accountRepository;
            _ProviceRepo = proviceRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]

        public IActionResult Index()
        {
            var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                           .Select(c => c.Value).SingleOrDefault();
            if (sId != null)
            {
                var user = _accountRepository.GetUser(sId);
                return View(user);
            }
            return View();
        }
        [HttpGet]
        [Route("changepassword")]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("changepassword")]
        public JsonResult ChangePassword(ChangePasswordModel model)
            {
            if (ModelState.IsValid)
            {

                var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                           .Select(c => c.Value).SingleOrDefault();
                if (sId != null)
                {
                    var username = _accountRepository.GetTaiKhoan(sId);
                    var check_pw = EncodeManager.VerifyHashedPassword(username.Password, model.Password);
                    var check_newpw = EncodeManager.VerifyHashedPassword(username.Password, model.New_Password);
                    if (check_pw == PasswordVerificationResult.Failed)
                    {
                        _toastNotification.AddErrorToastMessage("Mật khẩu nhập không chính xác vui lòng thử lại.");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "ChangePassword", model, null, "") });
                    }else if (check_newpw == PasswordVerificationResult.Success)
                    {
                        _toastNotification.AddErrorToastMessage("Mật khẩu mới không được giống mật khẩu cũ.");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "ChangePassword", model, null, "") });
                    }
                    else
                    {
                        bool blt = _accountRepository.ChangePassword(model, sId);
                        if (blt)
                        {
                            var user = _accountRepository.GetUser(sId);
                            _toastNotification.AddSuccessToastMessage("Thay đổi mật khẩu thành công thành công");
                            return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", user, null, "") });
                        }
                    }

                }
            }
            _toastNotification.AddErrorToastMessage("Thay đổi mật khẩu không thành công");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "ChangePassword", model, null, "") });
        }

        [Route("createaddress")]
        [HttpGet]
        public IActionResult CreateAddress()
        {

            return View();
        }
        [HttpPost]
        [Route("createaddress")]
        [ValidateAntiForgeryToken]
        public JsonResult CreateAddress(CreateAddressModel model)
        {
            if (ModelState.IsValid)
            {
                var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                           .Select(c => c.Value).SingleOrDefault();
                if (sId != null)
                {
                    bool blt = _accountRepository.CreateAddressUser(model, sId);
                    if (blt)
                    {
                        var user = _accountRepository.GetUser(sId);
                        _toastNotification.AddSuccessToastMessage("Thêm địa chỉ thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", user, null, "") });
                    }
                }
            }
            _toastNotification.AddErrorToastMessage("Thêm địa chỉ không thành công");
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "CreateAddress", model, null, "") });
        }
        [Route("edit")]
        public IActionResult Edit()
        {

            var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                           .Select(c => c.Value).SingleOrDefault();
            if (sId != null)
            {
                var user = _accountRepository.GetAccountModel(sId);
                ViewBag.Tinh = GetTinhs();
                ViewBag.Huyen = GetHuyens(user.MaTinh);
                ViewBag.Xa = GetXas(user.MaHuyen);
                return View(user);
            }
            return View();
        }
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EditAccountModel model)
        {
            var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                        .Select(c => c.Value).SingleOrDefault();

            if (ModelState.IsValid)
            {
                bool checkEmailNew = _accountRepository.IsEmailUser(model.Email, model.MaNguoiDung);
                if (!checkEmailNew)
                {
                    bool checkMailUser = _accountRepository.IsEmailUserNoExites(model.Email);
                    if (checkMailUser)
                    {
                        if (sId != null)
                        {
                            var users = _accountRepository.GetAccountModel(sId);
                            ViewBag.Tinh = GetTinhs();
                            ViewBag.Huyen = GetHuyen(users.MaHuyen);
                            ViewBag.Xa = GetXa(users.MaXa);
                        }
                        ModelState.AddModelError("Email", "Địa chỉ email này đã có rồi");
                        _toastNotification.AddErrorToastMessage("Cập nhật tài khoản không thành công");
                        return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
                    }
                }
                bool blt = _accountRepository.EditAccount(model, model.MaNguoiDung, (int)model.MaDiaChi);
                if (blt)
                {
                    var getuser = _accountRepository.GetUser(sId);
                    _toastNotification.AddSuccessToastMessage("Tài khoản cập nhật thành công");
                    return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", getuser, null, "") });
                }

            }
            var user = _accountRepository.GetAccountModel(sId);
            ViewBag.Tinh = GetTinhs();
            ViewBag.Huyen = GetHuyens(model.MaTinh);
            ViewBag.Xa = GetXas(model.MaHuyen);
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "edit", model, null, "") });
        }
        [HttpGet]
        [Route("editaddress")]
        public IActionResult EditAddress(int id)
        {
            var item = _accountRepository.GetEditAddressUser(id);
            ViewBag.Tinh = GetTinhs();
            ViewBag.Huyen = GetHuyens(item.MaTinh);
            ViewBag.Xa = GetXas(item.MaHuyen);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editaddress")]
        public JsonResult EditAddress(CreateAddressModel model)
        {
            if (ModelState.IsValid)
            {
                var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                           .Select(c => c.Value).SingleOrDefault();
                if (sId != null)
                {
                    bool blt = _accountRepository.EditAddressUser(model, sId);
                    if (blt)
                    {
                        var user = _accountRepository.GetUser(sId);
                        _toastNotification.AddSuccessToastMessage("Chỉnh sửa địa chỉ thành công");
                        return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", user, null, "") });
                    }
                }
            }
            _toastNotification.AddErrorToastMessage("Chỉnh sửa địa chỉ không thành công");
            ViewBag.Tinh = GetTinhs();
            ViewBag.Huyen = GetHuyens(model.MaTinh);
            ViewBag.Xa = GetXa(model.MaXa);
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "EditAddress", model, null, "") });
        }
        [HttpGet]
        [Route("deleteaddress")]
        public IActionResult DeleteAddress(int id)
        {
            var item = _accountRepository.GetEditAddressUser(id);
            return View(item);
        }
        [HttpPost]
        [Route("DeleteAddressConfirm")]

        public JsonResult DeleteAddressConfirm(int id)
        {

            var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                            .Select(c => c.Value).SingleOrDefault();
            var item = _accountRepository.GetEditAddressUser(id);
            if (sId != null)
            {
                bool blt = _accountRepository.DeleteAddressUser(id);
                if (blt)
                {
                    var user = _accountRepository.GetUser(sId);
                    _toastNotification.AddSuccessToastMessage("Xóa địa chỉ thành công");
                    return Json(new { isValid = true, html = RenderRazorView.RenderRazorViewToString(this, "_ViewAll", user, null, "") });
                }
            }
            return Json(new { isValid = false, html = RenderRazorView.RenderRazorViewToString(this, "DeleteAddress", item, null, "") });
        }
        [Route("Tinh")]
        public async Task<JsonResult> Tinh()
        {
            var cnt = await _ProviceRepo.GetProvinces();
            return new JsonResult(cnt);
        }
        private List<SelectListItem> GetTinhs()
        {
            var lstBrand = new List<SelectListItem>();
            List<Tinh> brands = _ProviceRepo.GetProvince();
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaTinh.ToString(),
                Text = ut.TenTinh
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn tỉnh----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        private List<SelectListItem> GetTinhs(string id)
        {
            var lstBrand = new List<SelectListItem>();
            List<Tinh> brands = _ProviceRepo.GetProvince(id);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaTinh.ToString(),
                Text = ut.TenTinh
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn tỉnh----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        [Route("Huyen")]
        public async Task<JsonResult> Huyen(string id)
        {
            var huyen = await _DistrictRepo.GetDistricts(id);
            return new JsonResult(huyen);
        }
        //Lay ra ten Huyen Cu The
        private List<SelectListItem> GetHuyen(string id)
        {
            var lstBrand = new List<SelectListItem>();
            List<Huyen> brands = _DistrictRepo.GetDistrict(id);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaHuyen.ToString(),
                Text = ut.TenHuyen
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn huyện----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        private List<SelectListItem> GetHuyens(string id)
        {
            var lstBrand = new List<SelectListItem>();
            List<Huyen> brands = _DistrictRepo.GetDistrict_MaTinh(id);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaHuyen.ToString(),
                Text = ut.TenHuyen
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn huyện----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        [Route("Xa")]
        public async Task<JsonResult> Xa(string id)
        {
            var xa = await _CommuneRepo.GetCommunes(id);
            return new JsonResult(xa);
        }
        private List<SelectListItem> GetXa(string id)
        {
            var lstBrand = new List<SelectListItem>();
            List<Xa> brands = _CommuneRepo.GetCommune(id);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaXa.ToString(),
                Text = ut.TenXa
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn xã----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        private List<SelectListItem> GetXas(string id)
        {
            var lstBrand = new List<SelectListItem>();
            List<Xa> brands = _CommuneRepo.GetCommune_MaHuyen(id);
            lstBrand = brands.Select(ut => new SelectListItem()
            {
                Value = ut.MaXa.ToString(),
                Text = ut.TenXa
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Chọn xã----",
            };
            lstBrand.Insert(0, defItem);
            return lstBrand;
        }
        // GET: Account/Register
        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            var Register = new RegisterModel();
            return View(Register);
        }
        //POST: Account/Register
        [Route("register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                bool checkmail = _accountRepository.IsAccountNoExists(register);
                bool checkusername = _accountRepository.IsUsernameNoExits(register.Username);
                if (checkusername)
                {
                    ModelState.AddModelError("Username", "Tài khoản này đã có rồi");
                    _toastNotification.AddErrorToastMessage("Tài khoản đăng ký không thành công");
                    return View(register);
                }
                else if (checkmail)
                {
                    ModelState.AddModelError("Email", "Địa chỉ email này đã có rồi");
                    _toastNotification.AddErrorToastMessage("Tài khoản đăng ký không thành công");
                    return View(register);
                }
                else
                {
                    await _accountRepository.RegisterAccount(register);
                    _toastNotification.AddSuccessToastMessage("Tài khoản đăng ký thành công");
                    return Redirect("login");
                }
            }
            _toastNotification.AddErrorToastMessage("Tài khoản đăng ký không thành công");
            return View(register);
        }
        //GET: account/login
        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        //POST: account/login
        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl)
        {
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;
            ViewData["ReturnUrl"] = "/";

            if (ModelState.IsValid)
            {
                bool check_tk = _accountRepository.IsUsernameNoExits(loginModel.Username);
                var account = _accountRepository.GetAccount(loginModel.Username);
                if (!check_tk)
                {
                    ModelState.AddModelError("Error", "Tài khoản hoặc mật khẩu không chính xác");
                    _toastNotification.AddErrorToastMessage("Đăng nhập không thành công");
                    return View(loginModel);
                }
                if (returnUrl != "/")
                {
                    var user = _accountRepository.GetUser(account.MaNguoiDung);
                    var check_pw = EncodeManager.VerifyHashedPassword(account.Password, loginModel.Password);

                    if (check_pw == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("IdNguoiDungLogin", account.MaNguoiDung);
                        var getRole = _accountRepository.GetRoles(account.Username);
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.HoNguoiDung + " " + user.TenNguoiDung));
                        claims.Add(new Claim(ClaimTypes.Sid, account.MaNguoiDung));
                        claims.Add(new Claim(ClaimTypes.Actor, account.AnhTk));
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            if (t.TenQuyen == "User")
                                claims.Add(new Claim(ClaimTypes.Role, "User"));
                            if (t.TenQuyen == "Emloyee")
                                claims.Add(new Claim(ClaimTypes.Role, "Emloyee"));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        if (!loginModel.Remember)
                        {
                            await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties
                            {
                                IsPersistent = false
                            });
                        }
                        else
                        {
                            await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddDays(3)
                            });
                        }
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                return Redirect(returnUrl);
                            if (t.TenQuyen == "User")
                                return Redirect(returnUrl);
                            if (t.TenQuyen == "Emloyee")
                                return Redirect(returnUrl);
                        }
                        return Redirect(returnUrl);
                    }
                    return Redirect("/admin");
                }
                else
                {
                    var check_pw = EncodeManager.VerifyHashedPassword(account.Password, loginModel.Password);
                    if (check_pw == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("IdNguoiDungLogin", account.MaNguoiDung);
                        var user = _accountRepository.GetUser(account.MaNguoiDung);
                        var getRole = _accountRepository.GetRoles(loginModel.Username);
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.HoNguoiDung + " " + user.TenNguoiDung));
                        claims.Add(new Claim(ClaimTypes.Sid, account.MaNguoiDung));
                        claims.Add(new Claim(ClaimTypes.Actor, account.AnhTk));
                        claims.Add(new Claim(ClaimTypes.Expired, loginModel.Remember.ToString()));
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            if (t.TenQuyen == "Employee")
                                claims.Add(new Claim(ClaimTypes.Role, "Emloyee"));
                            if (t.TenQuyen == "User")
                                claims.Add(new Claim(ClaimTypes.Role, "User"));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        if (!loginModel.Remember)
                        {
                            
                            await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties
                            {
                                IsPersistent = false
                            });
                        }else
                        {
                            await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddDays(3)
                            });
                        }
                        
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                return Redirect("/admin");
                            if (t.TenQuyen == "Employee")
                                return Redirect("/admin");
                            if (t.TenQuyen == "User")
                                return Redirect(returnUrl);

                        }
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Tài khoản hoặc mật khẩu không chính xác");
                        _toastNotification.AddErrorToastMessage("Đăng nhập không thành công");
                        return View(loginModel);
                    }
                }
            }
            return View(loginModel);
        }
        [HttpPost]
        [Route("account/fileupload")]
        public async Task<JsonResult> FileUpload(IFormFile formFile)
        {
            try
            {
                var sId = User.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                            .Select(c => c.Value).SingleOrDefault();
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/imagesUser", formFile.FileName);
                using var strem = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(strem);
                bool blt = _accountRepository.UpdateImage("/assets/images/imagesUser/" + formFile.FileName, sId);
                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error" });
            }
        }
        [Authorize]
        [Route("sigout")]
        public async Task<IActionResult> SigOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            _toastNotification.AddSuccessToastMessage("Đăng xuất thành công");
            return Redirect("/");
        }
    }
}
