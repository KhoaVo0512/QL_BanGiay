using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class UserRepo : IUser
    {
        private readonly QlyBanGiayContext _context;
        public UserRepo(QlyBanGiayContext context) 
        {
            _context = context;
        }
        public CheckOutModel GetUser(string id)
            {
            string item = id;
            var user = _context.NguoiDungs.Where(s => s.MaNguoiDung == id).FirstOrDefault();
            var getAddress = _context.DiaChis.Where(s=>s.MaNguoiDung == id).Include(s=>s.MaXaNavigation)
                .ThenInclude(s=>s.MaHuyenNavigation).ThenInclude(s=>s.MaTinhNavigation)
                .ToList();
            
            var checkoutModel = new CheckOutModel
            {
                Ho = user.HoNguoiDung,
                Ten = user.TenNguoiDung,
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
    }
}
