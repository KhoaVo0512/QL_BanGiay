using Microsoft.EntityFrameworkCore;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class BrandRepo : IBrand
    {
        private readonly QlyBanGiayContext _context;
        public BrandRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<NhanHieu> DoSort(List<NhanHieu> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namebrand")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenNhanHieu).ToList();
                else
                    items = items.OrderByDescending(n => n.TenNhanHieu).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.MaNhanHieu).ToList();
                else
                    items = items.OrderByDescending(d => d.MaNhanHieu).ToList();
            }

            return items;
        }
        public PaginatedList<NhanHieu> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<NhanHieu> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.NhanHieus.Where(n => n.TenNhanHieu.Contains(SearchText.ToLower()))
                    .Include(u => u.DongSanPhams).ToList();
            }
            else
                items = _context.NhanHieus.Include(u => u.DongSanPhams).ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<NhanHieu> retItems = new PaginatedList<NhanHieu>(items, pageIndex, pageSize);
            return retItems;
        }
    }
}
