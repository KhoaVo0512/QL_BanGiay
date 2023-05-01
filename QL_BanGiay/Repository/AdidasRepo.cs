using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Interface;

namespace QL_BanGiay.Repository
{
    public class AdidasRepo : IAdidas
    {
        private readonly QlyBanGiayContext _context;
        public AdidasRepo(QlyBanGiayContext context) 
        {
            _context = context;
        }
        private List<Giay> DoSort(List<Giay> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nameshoe")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.TenGiay).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.NgayCn).ToList();
                else
                    items = items.OrderBy(d => d.NgayCn).ToList();
            }

            return items;
        }
        public PaginatedList<Giay> GetItemsAdidas(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 12)
        {
            List<Giay> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.Giays.Where(ut => ut.TenGiay.ToLower().Contains(SearchText.ToLower())
                || ut.MaGiay.Contains(SearchText)
                || ut.GiaBan.ToString().Contains(SearchText)
                ).ToList();
            }
            else
                items = _context.Giays.Where(s=>s.MaDongSanPhamNavigation.MaNhanHieuNavigation.MaNhanHieu == 3 && s.GiaBan != null && s.TrangThai == true).ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Giay> retItems = new PaginatedList<Giay>(items, pageIndex, pageSize);

            return retItems;
        }

        public PaginatedList<Giay> GetItemsAdidasCollection(string namecollection,string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 12)
        {
            List<Giay> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.Giays.Where(ut => ut.MaDongSanPhamNavigation.TenDongSanPham == namecollection && ut.GiaBan != null && ut.TrangThai == true).ToList();
            }
            else
                items = _context.Giays.Where(s => s.MaDongSanPhamNavigation.TenDongSanPham == namecollection && s.GiaBan != null && s.TrangThai == true).ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Giay> retItems = new PaginatedList<Giay>(items, pageIndex, pageSize);

            return retItems;
        }
    }
}
