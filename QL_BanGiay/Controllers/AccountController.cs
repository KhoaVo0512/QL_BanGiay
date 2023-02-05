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
        private readonly QlyBanGiayContext _context;
        private readonly IAccount _accountRepository;

        public AccountController(QlyBanGiayContext context, IToastNotification toastNotification, IAccount accountRepository)
        {
            _toastNotification = toastNotification;
            _context = context;
            _accountRepository = accountRepository;
        }
        [Route("Tinh")]
        public JsonResult Tinh()
        {
            var cnt = _context.Tinhs.OrderBy(s => s.TenTinh).ToList();
            return new JsonResult(cnt);
        }
        [Route("Huyen")]
        public JsonResult Huyen(string id)
        {
            var huyen = _context.Huyens.Where(e => e.MaTinh == id).OrderBy(s => s.TenHuyen).ToList();
            return new JsonResult(huyen);
        }
        [Route("Xa")]
        public JsonResult Xa(string id)
        {
            var xa = _context.Xas.Where(e => e.MaHuyen == id).OrderBy(s => s.TenXa).ToList();
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
                var checkmail = _context.NguoiDungs.Where(s => s.Email == register.Email).FirstOrDefault();
                if (checkmail != null)
                {
                    ModelState.AddModelError(String.Empty, "Địa chỉ email này đã có rồi");
                    return View(register);
                }
                else
                {
                    await _accountRepository.RegisterAccount(register);
                    _toastNotification.AddSuccessToastMessage("Tài khoản đăng ký thành công");
                    return Redirect("login");
                }
            }
            //Ham so loi bat trong model
            //var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
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
                var check_tk = _context.NguoiDungs.Where(s => s.Email == loginModel.Email).FirstOrDefault();
                if (check_tk == null)
                {

                    ModelState.AddModelError("Error", "Tài khoản hoặc mật khẩu không chính xác");
                    return View(loginModel);
                }
                if (returnUrl != "/")
                {
                    var check_pw = EncodeManager.VerifyHashedPassword(check_tk.Password, loginModel.Password);
                    if (check_pw == PasswordVerificationResult.Success)
                    {
                        var check_quyen = (from n in _context.NguoiDungs
                                           join q in _context.QuyenCts
                                           on n.MaNguoiDung equals q.MaNguoiDung into table1
                                           from t in table1.DefaultIfEmpty()
                                           join d in _context.Quyens
                                           on t.MaQuyen equals d.MaQuyen
                                           where n.MaNguoiDung == check_tk.MaNguoiDung
                                           select new Quyen
                                           {
                                               TenQuyen = d.TenQuyen
                                           }).ToList();
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, check_tk.HoNguoiDung + " " + check_tk.TenNguoiDung));
                        foreach (var t in check_quyen)
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
                        foreach (var t in check_quyen)
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
                    var check_pw = EncodeManager.VerifyHashedPassword(check_tk.Password, loginModel.Password);
                    if (check_pw == PasswordVerificationResult.Success)
                    {
                        var check_quyen = (from n in _context.NguoiDungs
                                           join q in _context.QuyenCts
                                           on n.MaNguoiDung equals q.MaNguoiDung into table1
                                           from t in table1.DefaultIfEmpty()
                                           join d in _context.Quyens
                                           on t.MaQuyen equals d.MaQuyen
                                           where n.MaNguoiDung == check_tk.MaNguoiDung
                                           select new Quyen
                                           {
                                               TenQuyen = d.TenQuyen
                                           }).ToList();
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, check_tk.HoNguoiDung + " " + check_tk.TenNguoiDung));
                        foreach (var t in check_quyen)
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
                        foreach (var t in check_quyen)
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
                        return View(loginModel);
                    }
                }
            }
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
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
