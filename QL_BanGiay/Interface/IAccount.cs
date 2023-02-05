using QL_BanGiay.Models;

namespace QL_BanGiay.Interface
{
    public interface IAccount
    {
        Task RegisterAccount(RegisterModel register);
        Task Login(LoginModel login);
    }
}
