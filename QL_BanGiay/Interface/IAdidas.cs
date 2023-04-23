using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IAdidas
    {
        PaginatedList<Giay> GetItemsAdidas(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 6);
        PaginatedList<Giay> GetItemsAdidasCollection(string namecollection,string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 6);
    }
}
