using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IUser
    {
        CheckOutModel GetUser(string id);
    }
}
