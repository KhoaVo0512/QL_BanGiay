using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Interface
{
    public interface IAccount
    {
        Task RegisterAccount(RegisterModel register);
        Task Login(LoginModel login);
        bool IsAccountNoExists(RegisterModel account);
        bool IsEmailNoExists(LoginModel account);
        NguoiDung GetAccount(string email);
        List<Quyen> GetRoles(string Id);

    }
}
