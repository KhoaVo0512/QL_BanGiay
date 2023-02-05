using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface ISupplier
    {
        PaginatedList<DonViNhapHang> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
    }
}
