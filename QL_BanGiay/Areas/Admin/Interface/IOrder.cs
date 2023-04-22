using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IOrder
    {
        DonDat GetItem(string id);
        PaginatedList<DonDat> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        DonDat Delete(string id);
        Task<string> CreateHoaDon(DonDat item);
        EmailModel GetMail(string MaHD);
        int GetCountDonDat();
        double GetTotalInCome();
    }
}
