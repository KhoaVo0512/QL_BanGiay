using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class WareHouseRepo : IWareHouse
    {
        private readonly QlyBanGiayContext _context;
        public WareHouseRepo (QlyBanGiayContext context)
        {
            _context = context;
        }

        public KhoGiay GetItemWareHouse(string? maGiay, int? maSize)
        {
            var item = _context.KhoGiays.Where(s=>s.MaGiay ==maGiay && s.MaSize == maSize).FirstOrDefault();
            return item;
        }

        public bool IsShoeSizeNoExists(string? maGiay, int? maSize)
        {
            int ct = _context.KhoGiays.Where(s => s.MaGiay == maGiay && s.MaSize == maSize).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
