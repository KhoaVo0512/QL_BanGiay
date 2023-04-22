using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class WareHouseRepo : IWareHouse
    {
        private readonly QlyBanGiayContext _context;
        public WareHouseRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<KhoGiay> DoSort(List<KhoGiay> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nameshoe")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaGiayNavigation.TenGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.MaGiayNavigation.TenGiay).ToList();
            }
            else if (SortProperty.ToLower() == "idshoe")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.MaGiay).ToList();
            }
            else if (SortProperty.ToLower() == "size")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaSizeNavigation.TenSize).ToList();
                else
                    items = items.OrderByDescending(n => n.MaSizeNavigation.TenSize).ToList();
            }
            else if (SortProperty.ToLower() == "quantity")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.SoLuong).ToList();
                else
                    items = items.OrderByDescending(n => n.SoLuong).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaGiay).ToList();
                else
                    items = items.OrderBy(d => d.MaGiay).ToList();
            }

            return items;
        }
        public PaginatedList<KhoGiay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10)
        {
            List<KhoGiay> items;
            int SL;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.KhoGiays.Where(ut => ut.MaGiayNavigation.TenGiay.ToLower().Contains(SearchText.ToLower())
                || ut.MaGiay.Contains(SearchText)
                || ut.MaSizeNavigation.TenSize.Contains(SearchText)
                || ut.SoLuong.ToString().Contains(SearchText))
                                       .Include(s => s.MaGiayNavigation)
                                       .Include(s => s.MaSizeNavigation)
                                       .ToList();
            }
            else
                items = _context.KhoGiays
                    .Include(s => s.MaGiayNavigation)
                    .Include(s => s.MaSizeNavigation)
                    .ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<KhoGiay> retItems = new PaginatedList<KhoGiay>(items, pageIndex, pageSize);

            return retItems;
        }

        public KhoGiay GetItemWareHouse(string? maGiay, int? maSize)
        {
            var item = _context.KhoGiays.Where(s => s.MaGiay == maGiay && s.MaSize == maSize).FirstOrDefault();
            return item;
        }

        public bool IsShoeSizeNoExists(string? maGiay, int? maSize)
        {
            int ct = _context.KhoGiays.Where(s => s.MaGiay == maGiay && s.MaSize == maSize).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
