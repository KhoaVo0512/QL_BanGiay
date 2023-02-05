using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class ProduceRepo : IProduce
    {
        private readonly QlyBanGiayContext _context;
        public ProduceRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<NoiSanXuat> DoSort(List<NoiSanXuat> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nameproduce")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenNhaSanXuat).ToList();
                else
                    items = items.OrderByDescending(n => n.TenNhaSanXuat).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaNhaSanXuat).ToList();
                else
                    items = items.OrderBy(d => d.MaNhaSanXuat).ToList();
            }

            return items;
        }
        public PaginatedList<NoiSanXuat> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<NoiSanXuat> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.NoiSanXuats.Where(ut=>ut.TenNhaSanXuat.Contains(SearchText)).ToList();
            }
            else
                items = _context.NoiSanXuats.ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<NoiSanXuat> retItems = new PaginatedList<NoiSanXuat>(items, pageIndex, pageSize);

            return retItems;
        }
    }
}
