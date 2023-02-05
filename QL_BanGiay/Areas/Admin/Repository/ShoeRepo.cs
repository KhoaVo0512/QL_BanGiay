
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class ShoeRepo : IShoe
    {
        private readonly QlyBanGiayContext _context;
        public ShoeRepo(QlyBanGiayContext context)
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
            else if (SortProperty.ToLower() == "magiay")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.MaGiay).ToList();
                else
                    items = items.OrderByDescending(n => n.MaGiay).ToList();
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
        public PaginatedList<Giay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Giay> items;
            if (SearchText != "" && SearchText != null)
            {
                items = _context.Giays.ToList();
            }
            else
                items = _context.Giays.ToList();
            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<Giay> retItems = new PaginatedList<Giay>(items, pageIndex, pageSize);

            return retItems;
        }
    }
}
