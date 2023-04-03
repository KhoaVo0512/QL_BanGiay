using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class AddressRepo : IAddress
    {
        private readonly QlyBanGiayContext _context;
        public AddressRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        public string GetFullAddress(int? id)
        {
            var address = _context.DiaChis.Where(s => s.MaDiaChi == id).FirstOrDefault();
            var ward = _context.Xas.Where(s=>s.MaXa == address.MaXa).FirstOrDefault();
            var commune = _context.Huyens.Where(s=>s.MaHuyen == ward.MaHuyen).FirstOrDefault();
            var provide = _context.Tinhs.Where(s=>s.MaTinh == commune.MaTinh).FirstOrDefault();
            return address.TenDiaChi + ", " + ward.TenXa + " , " + commune.TenHuyen +" , " + provide.TenTinh;

        }
    }
}
