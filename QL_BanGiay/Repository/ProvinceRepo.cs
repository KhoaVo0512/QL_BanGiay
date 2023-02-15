using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Repository
{
    public class ProvinceRepo : IProvince
    {
        private readonly QlyBanGiayContext _context;
        public ProvinceRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        public Task<List<Tinh>> GetProvinces()
        {
            var provinces = _context.Tinhs.OrderBy(s=>s.TenTinh).ToList();
            return Task.FromResult(provinces);
        }
    }
}
