using Microsoft.Win32;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;

namespace QL_BanGiay.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly QlyBanGiayContext _context;
        public AccountRepository(QlyBanGiayContext context)
        {
            _context = context;
        }

        public NguoiDung GetAccount(string email)
        {
            var account = _context.NguoiDungs.Where(s => s.Email == email).FirstOrDefault();
            return account;
        }

        public List<Quyen> GetRoles(string Id)
        {
            var getRole = (from n in _context.NguoiDungs
                               join q in _context.QuyenCts
                               on n.MaNguoiDung equals q.MaNguoiDung into table1
                               from t in table1.DefaultIfEmpty()
                               join d in _context.Quyens
                               on t.MaQuyen equals d.MaQuyen
                               where n.MaNguoiDung == Id
                               select new Quyen
                               {
                                   TenQuyen = d.TenQuyen
                               }).ToList();
            return getRole;
        }

        public bool IsAccountNoExists(RegisterModel account)
        {
            var checkEmail = _context.NguoiDungs.Where(s => s.Email == account.Email).FirstOrDefault();
            if (checkEmail != null)
                return true;
            else
                return false;
        }

        public bool IsEmailNoExists(LoginModel account)
        {
            var email = _context.NguoiDungs.Where(s => s.Email == account.Email).FirstOrDefault();
            if (email != null)
                return true;
            else
                return false;
        }

        public async Task Login(LoginModel login)
        {
            
        }

        public async Task RegisterAccount(RegisterModel register)
        {

            var account = new NguoiDung()
            {
                MaNguoiDung = Guid.NewGuid().ToString().Substring(0, 10),
                HoNguoiDung = register.HoNguoiDung.Trim(),
                TenNguoiDung = register.TenNguoiDung.Trim(),
                Email = register.Email.Trim(),
                Sdt = register.Sdt.Trim(),
                GioiTinh = register.GioiTinh,
                CreateAt = DateTime.Now,
                Password = Convert.ToBase64String(EncodeManager.HashPasswordV2(register.Password.Trim()))
            };
            var diachi = new DiaChi()
            {
                MaNguoiDung = account.MaNguoiDung,
                TenDiaChi = register.DiaChi.Trim(),
                MaXa = register.MaXa
            };
            var quyen = new QuyenCt()
            {
                MaNguoiDung = account.MaNguoiDung,
                MaQuyen = 2
            };
            await _context.NguoiDungs.AddAsync(account);
            await _context.DiaChis.AddAsync(diachi);
            await _context.QuyenCts.AddAsync(quyen);
            await _context.SaveChangesAsync();
        }
    }
}
