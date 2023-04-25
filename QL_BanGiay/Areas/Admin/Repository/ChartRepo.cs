using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using System.Collections;

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
            DateTime StartDate = DateTime.Today.AddDays(-6);
            List<SplineChartData> data = _context.HoaDons.Where(s => s.NgayGiaoHd >= StartDate)
                .GroupBy(s => s.NgayGiaoHd.Value.Date)
                .Select(k => new SplineChartData()
                {
                    day = @Convert.ToDateTime(k.First().NgayGiaoHd).ToString("dd-MM"),
                    income = (int)k.Sum(l => l.TongTien)
                }).ToList();
            return data;
        }
        public List<SplineChartData> ExpenseSummary()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            List<SplineChartData> data = _context.NhapHangs.Where(s => s.NgayNhap >= StartDate)
                .GroupBy(s => s.NgayNhap.Value.Date).Select(k => new SplineChartData()
                {
                    day = @Convert.ToDateTime(k.First().NgayNhap).ToString("dd-MM"),
                    expense = (int)k.Sum(l => l.TongTien)
                }).ToList();
            return data;
        }

        public double? Total()
        {
            double? total = 0;
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var items = _context.HoaDons.Where(s => s.NgayGiaoHd >= StartDate && s.NgayGiaoHd <= EndDate).ToList();
            foreach (var item in items)
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
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayGiaoHd >= StartDate && s.MaHdNavigation.NgayGiaoHd <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 3)).ToList();
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
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayGiaoHd >= StartDate && s.MaHdNavigation.NgayGiaoHd <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 1)).ToList();
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
            var items = _context.HoaDonCts.Where(s => (s.MaHdNavigation.NgayGiaoHd >= StartDate && s.MaHdNavigation.NgayGiaoHd <= EndDate) && (s.MaGiayNavigation.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 2)).ToList();
            foreach (var item in items)
            {
                total += item.DonGia * item.SoLuong;
            }
            return total;
        }

        public IQueryable<QuantityModel> ListQuantityInCome()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Now;
            var quantityIncome = new List<KhoGiay>();
            var group = from p in _context.HoaDonCts.Include(s=>s.MaSizeNavigation).Include(s=>s.MaGiayNavigation)
                        where p.MaHdNavigation.NgayGiaoHd >= StartDate && p.MaHdNavigation.NgayGiaoHd <= EndDate
                        group p by new { MaGiay = p.MaGiay, MaSize = p.MaSize , TenSize = p.MaSizeNavigation.TenSize, TenGiay = p.MaGiayNavigation.TenGiay} into g
                        select new QuantityModel { MaGiay = g.Key.MaGiay,TenSize = g.Key.TenSize,TenGiay=g.Key.TenGiay, MaSize = (int)g.Key.MaSize, SoLuong = g.Sum(x => x.SoLuong) };
            var item = (IEnumerable)group;
            ReadQuantity();
            return group;
        }
        private void ReadQuantity()
        {
            var items = _context.HoaDonCts.ToList();
            var ListQuantity = new List<QuantityModel>();
            for (int i=0;i<items.Count; i++)
            {
                int? quantity = 0;
                for (int j = i;j< items.Count; j++)
                {
                    if (items[i].MaGiay == items[j].MaGiay && items[i].MaSize == items[j].MaSize)
                    {
                        quantity += items[i].SoLuong;
                    }
                }
                ListQuantity.Add(new QuantityModel { MaGiay = items[i].MaGiay, MaSize = (int)items[i].MaSize, SoLuong = quantity });
            }
            var item = ListQuantity;
        }
    }
}
