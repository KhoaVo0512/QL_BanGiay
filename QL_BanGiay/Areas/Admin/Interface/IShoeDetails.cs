using QL_BanGiay.Areas.Admin.Models;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IShoeDetails
    {
        bool Edit(ShoeDetails context);
        ShoeDetails GetItem(string shoeId);
    }
}
