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

        public List<Xa> GetCommune(string id)
        {
            var communes = _context.Xas.Where(s=>s.MaXa == id).OrderBy(s => s.TenXa).ToList();
            return communes;
        }

        public Task<List<Xa>> GetCommunes(string Id)
        {
            var communes = _context.Xas.Where(s => s.MaHuyen == Id).OrderBy(s=>s.TenXa).ToList();
            return Task.FromResult(communes);
        }

        public List<Xa> GetCommune_MaHuyen(string id)
        {
            var communes = _context.Xas.Where(s => s.MaHuyen == id).OrderBy(s => s.TenXa).ToList();
            return communes;
        }
    }
}
