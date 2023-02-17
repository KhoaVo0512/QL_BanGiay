
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IShoe 
    {
        Task<ShoeContext> Create(ShoeContext item);
        Task<ShoeContext> Edit(ShoeContext item);
        bool Delete(string id);
        ShoeContext GetItem(string id);
        List<Giay> GetItemsVans();
        List<Giay> GetItemsConverse();
        List<Giay> GetItemsNike();
        List<Giay> GetItemsAdidas();
        bool IsShoeNoExists(string magiay);
        PaginatedList<Giay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
    }
}
