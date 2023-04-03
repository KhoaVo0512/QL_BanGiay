using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IWareHouse
    {
        bool IsShoeSizeNoExists(string? maGiay, int? maSize);
        KhoGiay GetItemWareHouse(string? maGiay, int? maSize);
        PaginatedList<KhoGiay> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 10);

    }
}
