using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Repository
{
    public class DistrictRepo : IDistrict
    {
        private readonly QlyBanGiayContext _context;
        public DistrictRepo(QlyBanGiayContext context)
        {
            _context = context;
        }

        public Task<List<Huyen>> GetDistricts(string Id)
        {
            var districts = _context.Huyens.Where(s=>s.MaTinh == Id).OrderBy(s=>s.TenHuyen).ToList();
            return Task.FromResult(districts);
        }

        public List<Huyen> GetDistrict(string id)
        {
            var districts = _context.Huyens.Where(s=>s.MaHuyen == id).OrderBy(s => s.TenHuyen).ToList();
            return districts;
        }

        public List<Huyen> GetDistrict_MaTinh(string id)
        {
            var districts = _context.Huyens.Where(s => s.MaTinh == id).OrderBy(s => s.TenHuyen).ToList();
            return districts;
        }
    }
}
