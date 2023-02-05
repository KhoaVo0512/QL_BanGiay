
using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IPurchaseOrder
    {
        public string GetErrors();
        PaginatedList<NhapHang> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        NhapHang GetItem(int id);
        Task<NhapHang> Create(NhapHang nhaphang);
        bool Edit(NhapHang nhapHang);
        bool Delete(int id);
        public int GetNewPONumber();
    }
}
