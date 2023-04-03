
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IShoe 
    {
        Task<ShoeContext> Create(ShoeContext item);
        Task<EditShoeModel> Edit(EditShoeModel item);
        bool Delete(string id);
        EditShoeModel GetItem(string id);
        Giay GetItemCart(string id);
        List<Giay> GetItemsVans();
        List<Giay> GetItemsConverse();
        List<Giay> GetItemsNike();
        List<Giay> GetItemsAdidas();
        Giay GetItemWareHouse(string id);
        Giay GetItemProductDetails(string nameshoe);
        bool IsShoeNoExists(string magiay);
        bool IsNameShoeNoExists(string nameshoe);
        PaginatedList<Giay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
    }
}
