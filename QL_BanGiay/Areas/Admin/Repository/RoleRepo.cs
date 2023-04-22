using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Repository
{
    public class RoleRepo : IRole
    {
        private readonly QlyBanGiayContext _context;
        public RoleRepo(QlyBanGiayContext context) 
        {
            _context = context;
        }
        private List<Quyen> DoSort(List<Quyen> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "namerole")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.TenQuyen).ToList();
                else
                    items = items.OrderByDescending(n => n.TenQuyen).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.MaQuyen).ToList();
                else
                    items = items.OrderByDescending(d => d.TenQuyen).ToList();
            }

            return items;
        }
        public PaginatedList<Quyen> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            var items = _context.Quyens.ToList();
            items = DoSort(items, SortProperty, sortOrder);
            PaginatedList<Quyen> retItems = new PaginatedList<Quyen>(items, pageIndex, pageSize);
            return retItems;
        }
    }
}
