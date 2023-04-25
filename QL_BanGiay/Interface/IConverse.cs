using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IConverse
    {
        PaginatedList<Giay> GetItemsConverse(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 6);
        PaginatedList<Giay> GetItemsConverseCollection(string namecollection, string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 6);
    }
}
