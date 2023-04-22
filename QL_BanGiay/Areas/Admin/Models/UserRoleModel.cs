using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Models
{
    public class UserRoleModel
    {
        public string? MaNguoiDung { get;set; }
        public string? UserName { get; set; }
        public string? HoTen { get; set; }
        public List<QuyenCt>? RoleName { get; set;}
        public List<ManageUserRolesViewModel> manageUserRoles { get; set; }

    }
}
