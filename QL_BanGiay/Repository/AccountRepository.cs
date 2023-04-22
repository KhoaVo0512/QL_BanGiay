using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using QL_BanGiay.Data;
using QL_BanGiay.Helps;
using QL_BanGiay.Interface;
using QL_BanGiay.Models;
using System.Text;
using Windows.System;

namespace QL_BanGiay.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly QlyBanGiayContext _context;
        public AccountRepository(QlyBanGiayContext context)
        {
            _context = context;
        }

        public async Task<DiaChi> CreateDiaChiDD(CheckOutModel model, string id)
        {
            var diachi = new DiaChi
            {
                MaNguoiDung = id,
                MaXa = model.MaXa,
                TenDiaChi = model.DiaChi
            };
            await _context.DiaChis.AddAsync(diachi);
            await _context.SaveChangesAsync();
            return diachi;
        }

        public EditAccountModel GetAccountModel(string model)
        {
            var user = _context.NguoiDungs.Where(s => s.MaNguoiDung == model).Include(s => s.TaiKhoans).FirstOrDefault();
            var address = _context.DiaChis.Where(s => s.MaNguoiDung == user.MaNguoiDung)
                .Include(s => s.MaXaNavigation)
                .ThenInclude(s => s.MaHuyenNavigation)
                .ThenInclude(s => s.MaTinhNavigation).FirstOrDefault();
            var item = new EditAccountModel
            {
                MaDiaChi = address.MaDiaChi,
                MaNguoiDung = user.MaNguoiDung,
                HoNguoiDung = user.HoNguoiDung.Trim(),
                TenNguoiDung = user.TenNguoiDung.Trim(),
                Email = user.Email.Trim(),
                Sdt = user.Sdt.Trim(),
                DiaChi = address.TenDiaChi.Trim(),
                MaTinh = address.MaXaNavigation.MaHuyenNavigation.MaTinhNavigation.MaTinh,
                MaHuyen = address.MaXaNavigation.MaHuyen,
                MaXa = address.MaXa
            };
            return item;
        }

        public TaiKhoan GetAccount(string username)
        {
            var account = _context.TaiKhoans.Where(s => s.Username == username).FirstOrDefault();
            return account;
        }

        public List<Quyen> GetRoles(string Id)
        {
            var getRole = (from n in _context.TaiKhoans
                           join q in _context.QuyenCts
                           on n.Username equals q.UserName into table1
                           from t in table1.DefaultIfEmpty()
                           join d in _context.Quyens
                           on t.MaQuyen equals d.MaQuyen
                           where n.Username == Id
                           select new Quyen
                           {
                               TenQuyen = d.TenQuyen
                           }).ToList();
            return getRole;
        }

        public NguoiDung GetUser(string id)
        {
            var user = _context.NguoiDungs.Where(s=>s.MaNguoiDung == id).Include(s=>s.TaiKhoans).Include(s=>s.DonDats).ThenInclude(s=>s.DonDatCts).FirstOrDefault();
            var address = _context.DiaChis.Where(s => s.MaNguoiDung == user.MaNguoiDung)
                .Include(s=>s.MaXaNavigation)
                .ThenInclude(s=>s.MaHuyenNavigation)
                .ThenInclude(s=>s.MaTinhNavigation).FirstOrDefault();
            user.Address = address.TenDiaChi + ", " + address.MaXaNavigation.TenXa + ", " + address.MaXaNavigation.MaHuyenNavigation.TenHuyen
                + ", " + address.MaXaNavigation.MaHuyenNavigation.MaTinhNavigation.TenTinh;
            var getAddressAll = _context.DiaChis.Where(s => s.MaNguoiDung == id).Include(s => s.MaXaNavigation)
               .ThenInclude(s => s.MaHuyenNavigation).ThenInclude(s => s.MaTinhNavigation)
               .ToList();
            user.DiasChiUser = new List<string>();
            user.IdDiaChi = new List<int>();
            for (int i = 0; i < getAddressAll.Count; i++)
            {
                string addressAll = "";
                addressAll = getAddressAll[i].TenDiaChi + ", " + getAddressAll[i].MaXaNavigation.TenXa
                    + ", " + getAddressAll[i].MaXaNavigation.MaHuyenNavigation.TenHuyen
                    + ", " + getAddressAll[i].MaXaNavigation.MaHuyenNavigation.MaTinhNavigation.TenTinh + ".";
                user.DiasChiUser.Add(addressAll);
                user.IdDiaChi.Add(getAddressAll[i].MaDiaChi);
            }
            for (int i=0;i< user.DonDats.Count;i ++)
            {
                double? total = 0;
                for (int j = 0; j < user.DonDats[i].DonDatCts.Count;j++)
                {
                    total += user.DonDats[i].DonDatCts[j].DonGia * user.DonDats[i].DonDatCts[j].SoLuong;
                }
                user.DonDats[i].Total = (int?)total;
            }
            return user;

        }

        public bool IsAccountNoExists(RegisterModel account)
        {
            var checkEmail = _context.NguoiDungs.Where(s => s.Email == account.Email).FirstOrDefault();
            if (checkEmail != null)
                return true;
            else
                return false;
        }

        public bool IsUsernameNoExits(string account)
        {
            var username = _context.TaiKhoans.Where(s => s.Username == account).FirstOrDefault();
            if (username != null) return true;
            else return false;
        }
        public async Task<string> NguoiDung(CheckOutModel model, string note)
        {
            int length = 10;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(10 * flt));
                letter = Convert.ToChar(shift + 48);
                str_build.Append(letter);
            }
            var madd = str_build.ToString();
            var getWard = _context.Xas.Where(s=>s.MaXa == model.MaXa).FirstOrDefault();
            var getCommune = _context.Huyens.Where(s=>s.MaHuyen == model.MaHuyen).FirstOrDefault();
            var getProvide = _context.Tinhs.Where(s=>s.MaTinh == model.MaTinh).FirstOrDefault();
            var user = new NguoiDung
            {

                MaNguoiDung = Guid.NewGuid().ToString().Substring(0, 10),
                HoNguoiDung = model.Ho,
                TenNguoiDung = model.Ten,
                Email = model.Email,
                Sdt = model.Sdt,
                CreateAt = DateTime.Now
            };
            var dondat = new DonDat
            {
                MaDonDat = madd,
                MaNguoiDung = user.MaNguoiDung,
                NgayDat = DateTime.Now,
                TrangThai = 0,
                GhiChu = note,
                MaVnpay = "0",
                DaThanhToan = false,
                DiaChiNhan = model.DiaChi + ", " + getWard.TenXa + ", " + getCommune.TenHuyen +", " + getProvide.TenTinh
            };
            await _context.NguoiDungs.AddAsync(user);
            await _context.DonDats.AddAsync(dondat);
            await _context.SaveChangesAsync();
            return user.MaNguoiDung;
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
                CreateAt = DateTime.Now,
            };
            var taikhoan = new TaiKhoan()
            {
                Username = register.Username.Trim(),
                Password = Convert.ToBase64String(EncodeManager.HashPasswordV2(register.Password.Trim())),
                MaNguoiDung = account.MaNguoiDung,
                AnhTk = "/assets/images/imagesUser/user.png"
            };
            var diachi = new DiaChi()
            {
                MaNguoiDung = account.MaNguoiDung,
                TenDiaChi = register.DiaChi.Trim(),
                MaXa = register.MaXa
            };
            var quyen = new QuyenCt()
            {
                UserName = register.Username,
                MaQuyen = 2
            };
            await _context.NguoiDungs.AddAsync(account);
            await _context.TaiKhoans.AddAsync(taikhoan);
            await _context.DiaChis.AddAsync(diachi);
            await _context.QuyenCts.AddAsync(quyen);
            await _context.SaveChangesAsync();
        }

        public bool EditAccount(EditAccountModel model, string id, int idAddress)
        {
            var getUser = _context.NguoiDungs.Where(s=>s.MaNguoiDung == id).FirstOrDefault();
            var getAddress = _context.DiaChis.Where(s=>s.MaDiaChi == idAddress).FirstOrDefault();
            if (getUser != null && getAddress != null)
            {
                getUser.HoNguoiDung = model.HoNguoiDung;
                getUser.TenNguoiDung = model.TenNguoiDung;
                getUser.Email = model.Email;
                getUser.Sdt = model.Sdt;
                getAddress.TenDiaChi = model.DiaChi;
                getAddress.MaXa = model.MaXa;
                _context.NguoiDungs.Update(getUser);
                _context.DiaChis.Update(getAddress);
                _context.SaveChanges();
            }
            return true;
        }

        public bool IsEmailUserNoExites(string email)
        {
            bool blt = false;
            var users = _context.TaiKhoans.Include(s => s.MaNguoiDungNavigation).ToList();
            for (int i=0;i< users.Count; i++)
            {
                if (users[i].MaNguoiDungNavigation.Email == email)
                {
                    blt = true;
                    break;
                }
            }
            if (blt)
                return true;
            else return false;
        }

        public bool IsEmailUser(string email, string id)
        {
            var user =_context.NguoiDungs.Where(s=>s.MaNguoiDung == id).FirstOrDefault();
            if (user.Email == email) return true;
            else return false;
        }

        public bool CreateAddressUser(CreateAddressModel model, string id)
        {
            try
            {
                var item = new DiaChi
                {
                    MaNguoiDung = id,
                    MaXa = model.MaXa,
                    TenDiaChi = model.DiaChi
                };
                _context.DiaChis.Add(item);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
           
        }

        public CreateAddressModel GetEditAddressUser(int id)
        {
            var address = _context.DiaChis.Where(s=>s.MaDiaChi == id)
                .Include(s=>s.MaXaNavigation)
                .ThenInclude(s=>s.MaHuyenNavigation)
                .ThenInclude(s=>s.MaTinhNavigation).FirstOrDefault();
            var item = new CreateAddressModel
            {
                MaDiaChi = address.MaDiaChi,
                MaXa = address.MaXa,
                DiaChi = address.TenDiaChi,
                MaHuyen = address.MaXaNavigation.MaHuyenNavigation.MaHuyen,
                MaTinh = address.MaXaNavigation.MaHuyenNavigation.MaTinhNavigation.MaTinh
            };
            return item;
        }

        public bool EditAddressUser(CreateAddressModel model, string id)
        {
            try
            {
                var getAddress = _context.DiaChis.Where(s => s.MaNguoiDung == id).FirstOrDefault();
                getAddress.TenDiaChi = model.DiaChi;
                getAddress.MaXa = model.MaXa;
                _context.DiaChis.Update(getAddress);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteAddressUser(int id)
        {
            try
            {
                var address = _context.DiaChis.Where(s => s.MaDiaChi == id).FirstOrDefault();
                _context.DiaChis.Remove(address);
                _context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool ChangePassword(ChangePasswordModel model, string id)
        {
            try
            {
                var user = _context.TaiKhoans.Where(s => s.MaNguoiDung == id).FirstOrDefault();
                user.Password = Convert.ToBase64String(EncodeManager.HashPasswordV2(model.New_Password.Trim()));
                _context.TaiKhoans.Update(user);
                _context.SaveChanges();
                return true;
            }catch(Exception ex) { return false; }
        }

        public TaiKhoan GetTaiKhoan(string id)
        {
            var account = _context.TaiKhoans.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            return account;
        }

        public bool UpdateImage(string url, string id)
        {
            var account = _context.TaiKhoans.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            if (account != null)
            {
                account.AnhTk = url;
                _context.TaiKhoans.Update(account);
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
    }
}
