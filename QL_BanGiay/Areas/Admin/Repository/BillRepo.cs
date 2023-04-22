using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class BillRepo : IBill
    {
        private readonly QlyBanGiayContext _context;
        public BillRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<HoaDon> DoSort(List<HoaDon> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "price")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TongTien).ToList();
                else
                    items = items.OrderByDescending(n => n.TongTien).ToList();
            }
            else if (SortProperty.ToLower() == "date")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.NgayLapDh).ToList();
                else
                    items = items.OrderByDescending(n => n.NgayLapDh).ToList();
            }
            else if (SortProperty.ToLower() == "idcheckout")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaHd).ToList();
                else
                    items = items.OrderByDescending(n => n.MaHd).ToList();
            }
            else if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
                else
                    items = items.OrderByDescending(n => n.MaNguoiDungNavigation.TenNguoiDung).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.NgayGiaoHd).ToList();
                else
                    items = items.OrderBy(d => d.NgayGiaoHd).ToList();
            }

            return items;
        }
        public PaginatedList<HoaDon> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<HoaDon> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.HoaDons.Where(ut => ut.MaHd.ToLower().Contains(SearchText.ToLower()) ||
                ut.MaNguoiDungNavigation.TenNguoiDung.ToLower().Contains(SearchText.ToLower()) || 
                ut.MaNguoiDungNavigation.HoNguoiDung.ToLower().Contains(SearchText.ToLower()) ||
                ut.MaHd.Contains(SearchText) ||
                ut.TongTien.ToString().Contains(SearchText))
                .Include(s => s.MaNguoiDungNavigation)
                .ThenInclude(s=>s.DiaChis)
                .ThenInclude(t=>t.MaXaNavigation)
                .ThenInclude(h=>h.MaHuyenNavigation)
                .ThenInclude(t=>t.MaTinhNavigation)
                .Include(s=>s.HoaDonCts).ToList();
            }
            else
                items = _context.HoaDons
                .Include(s => s.MaNguoiDungNavigation)
                .ThenInclude(s => s.DiaChis)
                .ThenInclude(t => t.MaXaNavigation)
                .Include(s => s.HoaDonCts).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<HoaDon> retItems = new PaginatedList<HoaDon>(items, pageIndex, pageSize);
            return retItems;
        }

        public bool ChangeBill(string mahd, string matrangthai)
        {
            try
            {
                var item = _context.HoaDons.Where(s => s.MaHd == mahd).FirstOrDefault();
                if (matrangthai == "2")
                {
                    item.NgayGiaoHd = DateTime.Now;
                }
                _context.HoaDons.Update(item);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public HoaDon GetItem(string id)
        {
            HoaDon? item = _context.HoaDons.Where(s=>s.MaHd == id)
                .Include(s=>s.MaNguoiDungNavigation)
                .Include(h => h.HoaDonCts).ThenInclude(g => g.MaGiayNavigation)
                .FirstOrDefault();
            item.HoaDonCts.ForEach(p => p.Description = p.MaGiayNavigation.TenGiay);
            item.HoaDonCts.ForEach(p => p.Total = (decimal)(p.SoLuong * p.DonGia));
            item.TenKH = item.MaNguoiDungNavigation.HoNguoiDung + " " + item.MaNguoiDungNavigation.TenNguoiDung;
            item.Sdt = item.MaNguoiDungNavigation.Sdt;
            item.Email = item.MaNguoiDungNavigation.Email;
            return item;


        }
    }
}
