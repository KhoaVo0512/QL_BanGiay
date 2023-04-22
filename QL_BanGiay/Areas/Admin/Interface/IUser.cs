using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IUser
    {
        bool EditUserRole(string username, int idRole);
        
        CheckOutModel GetUser(string id);
        int GetUserCount();
        UserRoleModel UserRole(string maND);
        PaginatedList<TaiKhoan> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);
        TaiKhoan GetUserDetails(string id);
    }
}
