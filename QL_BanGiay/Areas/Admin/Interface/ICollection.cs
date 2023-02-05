using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface ICollection
    {
        PaginatedList<CollectionModel> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        
        Task<DongSanPham> Create(DongSanPham dongsanpham);
        Task<int> Delete(int id);
        DongSanPham GetItem(int? id);
        Task<DongSanPham> Edit(DongSanPham dongsanpham);
    }
}
