using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class ChartRepo : IChart
    {
        private readonly QlyBanGiayContext _context;
        public ChartRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        public List<SplineChartData> IncomeSummary()
        {
            List<SplineChartData> data = _context.HoaDons.GroupBy(s=>s.NgayGiaoHd).Select(k=> new SplineChartData()
            {
                day = @Convert.ToDateTime(k.First().NgayLapDh).ToString("dd--MM"),
                income = (int)k.Sum(l=>l.TongTien)
            }).ToList();
            return data;
        }
        public List<SplineChartData> ExpenseSummary()
        {
            List<SplineChartData> data = _context.NhapHangs.GroupBy(s => s.NgayNhap).Select(k => new SplineChartData()
            {
                day = @Convert.ToDateTime(k.First().NgayNhap).ToString("dd--MM"),
                expense = (int)k.Sum(l => l.TongTien)
            }).ToList();
            return data;
        }

        public double? Total()
        {
            double? total = 0;
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var items = _context.HoaDons.Where(s=>s.NgayLapDh >= StartDate && s.NgayLapDh <= EndDate).ToList();
            foreach(var item in items)
            {
                total += item.TongTien;
            }
            return total;
        }

        public double? TotalAdidas()
        {
            double? total = 0;
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayLapDh >= StartDate && s.MaHdNavigation.NgayLapDh <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 3)).ToList();
            foreach (var item in items)
            {
                total += item.DonGia * item.SoLuong;
            }
            return total;
        }

        public double? TotalConverse()
        {
            double? total = 0;
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayLapDh >= StartDate && s.MaHdNavigation.NgayLapDh <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 1)).ToList();
            foreach (var item in items)
            {
                total += item.DonGia * item.SoLuong;
            }
            return total; 
        }
        public double? TotalVans()
        {
            double? total = 0;
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayLapDh >= StartDate && s.MaHdNavigation.NgayLapDh <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 2)).ToList();
            foreach (var item in items)
            {
                total += item.DonGia * item.SoLuong;
            }
            return total;
        }
    }
}
