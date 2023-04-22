using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Interface
{
    public interface IAccount
    {
        Task RegisterAccount(RegisterModel register);
        bool IsAccountNoExists(RegisterModel account);
        bool IsUsernameNoExits(string account);
        bool IsEmailUserNoExites(string email);
        bool IsEmailUser(string email, string id);
        Task<string> NguoiDung(CheckOutModel model, string note);
        Task<DiaChi> CreateDiaChiDD(CheckOutModel model, string id);
        bool CreateAddressUser(CreateAddressModel model, string id);
        bool EditAddressUser(CreateAddressModel model, string id);
        CreateAddressModel GetEditAddressUser(int id);
        TaiKhoan GetAccount(string email);
        NguoiDung GetUser(string id);
        EditAccountModel GetAccountModel(string model);
        bool EditAccount(EditAccountModel model, string id, int idAddress);
        List<Quyen> GetRoles(string Id);
        bool DeleteAddressUser(int id);
        bool ChangePassword(ChangePasswordModel model, string id);
        bool UpdateImage(string url, string id);
        TaiKhoan GetTaiKhoan(string id);

    }
}
