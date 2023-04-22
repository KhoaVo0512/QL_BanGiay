using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Models;
using System.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class UserRepo : IUser
    {
        private readonly QlyBanGiayContext _context;
        private readonly IAddress _AddressRepo;

        public UserRepo(QlyBanGiayContext context, IAddress addressRepo)
        {
            _context = context;
            _AddressRepo = addressRepo;
        }
        private List<TaiKhoan> DoSort(List<TaiKhoan> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nameusername")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.Username).ToList();
                else
                    items = items.OrderByDescending(n => n.Username).ToList();
            }
            else if (SortProperty.ToLower() == "iduser")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDung).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDung).ToList();
            }
            else if (SortProperty.ToLower() == "nameuser")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
            }
            else if (SortProperty.ToLower() == "date")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDungNavigation.CreateAt).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDungNavigation.CreateAt).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.Username).ToList();
                else
                    items = items.OrderByDescending(d => d.Username).ToList();
            }

            return items;
        }
        public PaginatedList<TaiKhoan> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<TaiKhoan> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.TaiKhoans.Include(s => s.MaNguoiDungNavigation)
                    .Where(s=>s.MaNguoiDungNavigation.TenNguoiDung.ToLower().Contains(SearchText.ToLower())
                    || s.MaNguoiDung.Contains(SearchText.ToLower())
                    || s.Username.Contains(SearchText)).ToList();
            }
            else
                items = _context.TaiKhoans.Include(s => s.MaNguoiDungNavigation).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<TaiKhoan> retItems = new PaginatedList<TaiKhoan>(items, pageIndex, pageSize);
            return retItems;
        }

        public CheckOutModel GetUser(string id)
        {
            string item = id;
            var user = _context.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            var getAddress = _context.DiaChis.Where(s => s.MaNguoiDung == id).Include(s => s.MaXaNavigation)
                .ThenInclude(s => s.MaHuyenNavigation).ThenInclude(s => s.MaTinhNavigation)
                .ToList();

            var checkoutModel = new CheckOutModel
            {
                Ho = user.HoNguoiDung,
                Ten = user.TenNguoiDung,
                Email = user.Email,
                Sdt = user.Sdt
            };
            checkoutModel.DiaChis = new List<string>();
            checkoutModel.IdDiaChi = new List<int>();
            for (int i = 0; i < getAddress.Count; i++)
            {
                string address = "";
                address = getAddress[i].TenDiaChi + ", " + getAddress[i].MaXaNavigation.TenXa
                    + ", " + getAddress[i].MaXaNavigation.MaHuyenNavigation.TenHuyen
                    + ", " + getAddress[i].MaXaNavigation.MaHuyenNavigation.MaTinhNavigation.TenTinh + ".";
                checkoutModel.DiaChis.Add(address);
                checkoutModel.IdDiaChi.Add(getAddress[i].MaDiaChi);
            }
            return checkoutModel;
        }

        public int GetUserCount()
        {
            //Lay 7 ngay gan nhat
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;            
            var userCount = _context.TaiKhoans.Include(s => s.MaNguoiDungNavigation)
                .Where(s=>s.MaNguoiDungNavigation.CreateAt >=StartDate && s.MaNguoiDungNavigation.CreateAt <= EndDate).Count();           
            return userCount;
           
        }

        public TaiKhoan GetUserDetails(string id)
        {
            var user = _context.TaiKhoans.Where(s=>s.MaNguoiDung == id).Include(s=>s.MaNguoiDungNavigation).FirstOrDefault();
            user.HoVaTen = user.MaNguoiDungNavigation.HoNguoiDung + " " + user.MaNguoiDungNavigation.TenNguoiDung;
            var address = _context.DiaChis.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            user.DiaChi = _AddressRepo.GetFullAddress(address.MaDiaChi);
            return user;
        }

        public bool EditUserRole(string username, int idRole)
        {
            try
            {
                var role = new QuyenCt
                {
                    UserName = username,
                    MaQuyen = idRole
                };
                _context.QuyenCts.Add(role);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
          
        }

        public UserRoleModel UserRole(string maND)
        {
            var getAccount = _context.TaiKhoans.Where(s=>s.MaNguoiDung == maND).Include(s=>s.MaNguoiDungNavigation).FirstOrDefault();
            var getRole = _context.QuyenCts.Where(s=>s.UserName == getAccount.Username).ToList();
            var model = new UserRoleModel
            {
                MaNguoiDung = getAccount.MaNguoiDung,
                UserName = getAccount.Username,
                HoTen = getAccount.MaNguoiDungNavigation.HoNguoiDung + " " + getAccount.MaNguoiDungNavigation.TenNguoiDung,
                RoleName = getRole
            };
            return model;

        }
    }
}
