using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IVans
    {
        PaginatedList<Giay> GetItemsVans(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 12);
        PaginatedList<Giay> GetItemsVansCollection(string namecollection, string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 12);
    }
}
