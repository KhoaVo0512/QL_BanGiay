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
