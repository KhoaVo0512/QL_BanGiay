using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface ISupplier
    {
        bool CreateSupplier(DonViNhapHang model);
        bool EditSupplier(DonViNhapHang model);
        DonViNhapHang GetItem(int id);
        PaginatedList<DonViNhapHang> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
    }
}
