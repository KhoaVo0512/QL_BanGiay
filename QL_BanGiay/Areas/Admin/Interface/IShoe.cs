
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IShoe 
    {
        Task<ShoeContext> Create(ShoeContext item);
        bool IsShoeNoExists(string magiay);
        PaginatedList<Giay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
    }
}
