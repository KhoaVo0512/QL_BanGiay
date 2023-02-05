using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class SizeRepo : ISize
    {
        private readonly QlyBanGiayContext _context;
        public SizeRepo(QlyBanGiayContext context)
        {
            _context = context;
        }
        private List<SizeGiay> DoSort(List<SizeGiay> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namesize")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenSize).ToList();
                else
                    items = items.OrderByDescending(n => n.TenSize).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderByDescending(d => d.MaSize).ToList();
                else
                    items = items.OrderBy(d => d.MaSize).ToList();
            }

            return items;
        }
        public PaginatedList<SizeGiay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<SizeGiay> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.SizeGiays.ToList();
            }
            else
                items = _context.SizeGiays.ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<SizeGiay> retItems = new PaginatedList<SizeGiay>(items, pageIndex, pageSize);

            return retItems;
        }
    }
}
