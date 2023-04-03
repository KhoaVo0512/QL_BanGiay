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

        public List<Tinh> GetProvince()
        {
            var provinces = _context.Tinhs.OrderBy(s => s.TenTinh).ToList();
            return provinces;
        }

        public List<Tinh> GetProvince(string id)
        {
            var provinces = _context.Tinhs.Where(s=>s.MaTinh == id).ToList();
            return provinces;
        }

        public Task<List<Tinh>> GetProvinces()
        {
            var provinces = _context.Tinhs.OrderBy(s=>s.TenTinh).ToList();
            return Task.FromResult(provinces);
        }
    }
}
