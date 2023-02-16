using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;
using System.Security.Claims;

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

        public AccountController(IToastNotification toastNotification, IAccount accountRepository
            , IProvince proviceRepo, IDistrict district, ICommune commune)
        {
            _DistrictRepo = district;
            _CommuneRepo = commune;
            _toastNotification = toastNotification;
            _accountRepository = accountRepository;
            _ProviceRepo = proviceRepo;
        }
        [Route("Tinh")]
        public async Task<JsonResult> Tinh()
        {
            var cnt = await _ProviceRepo.GetProvinces();
            return new JsonResult(cnt);
        }
        [Route("Huyen")]
        public async Task<JsonResult> Huyen(string id)
        {
            var huyen = await _DistrictRepo.GetDistricts(id);
            return new JsonResult(huyen);
        }
        [Route("Xa")]
        public async Task<JsonResult> Xa(string id)
        {
            var xa = await _CommuneRepo.GetCommunes(id);
            return new JsonResult(xa);
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
                if (checkmail)
                {
                    ModelState.AddModelError(String.Empty, "Địa chỉ email này đã có rồi");
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
            ViewData["ReturnUrl"] = "index";

            if (ModelState.IsValid)
            {
                bool check_tk = _accountRepository.IsEmailNoExists(loginModel);
                var account = _accountRepository.GetAccount(loginModel.Email);
                if (!check_tk)
                {
                    ModelState.AddModelError("Error", "Tài khoản hoặc mật khẩu không chính xác");
                    _toastNotification.AddErrorToastMessage("Đăng nhập không thành công");
                    return View(loginModel);
                }
                if (returnUrl != "/")
                {
                    var check_pw = EncodeManager.VerifyHashedPassword(account.Password, loginModel.Password);
                    if (check_pw == PasswordVerificationResult.Success)
                    {
                        var getRole = _accountRepository.GetRoles(account.MaNguoiDung);
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, account.HoNguoiDung + " " + account.TenNguoiDung));
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
                        await HttpContext.SignInAsync(claimsPrincipal);
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                return Redirect("/admin");
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
                        var getRole = _accountRepository.GetRoles(account.MaNguoiDung);
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, account.HoNguoiDung + " " + account.TenNguoiDung));
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                            if (t.TenQuyen == "Emloyee")
                                claims.Add(new Claim(ClaimTypes.Role, "Emloyee"));
                            if (t.TenQuyen == "User")
                                claims.Add(new Claim(ClaimTypes.Role, "User"));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        foreach (var t in getRole)
                        {
                            if (t.TenQuyen == "Admin")
                                return Redirect("/admin");
                            if (t.TenQuyen == "Emloyee")
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
            ModelState.AddModelError(String.Empty, "Tài khoản hoặc mật khẩu không chính xác");
            _toastNotification.AddErrorToastMessage("Đăng nhập không thành công");
            return View(loginModel);
        }
        [Authorize]
        public async Task<IActionResult> SigOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
