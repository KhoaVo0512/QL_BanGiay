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
    }
}
