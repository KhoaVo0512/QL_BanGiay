using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Repository
{
    public class CommuneRepo : ICommune
    {
        private readonly QlyBanGiayContext _context;
        public CommuneRepo (QlyBanGiayContext context)
        {
            _context = context;
        }
        public Task<List<Xa>> GetCommunes(string Id)
        {
            var communes = _context.Xas.Where(s => s.MaHuyen == Id).OrderBy(s=>s.TenXa).ToList();
            return Task.FromResult(communes);
        }
    }
}
