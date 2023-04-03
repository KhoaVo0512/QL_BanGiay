using QL_BanGiay.Data;
using QL_BanGiay.Models;

namespace QL_BanGiay.Interface
{
    public interface ICheckout
    {
        DonDat CreateCheckout(ProductModel[] Stock, string id);
        bool EditCheckOut(string? idMaDonDat, long IdVNPay);
        Task<bool> DeleteNguoiDungAndDonDat(string? idDonDat, string idNguoiDung);
        Task<int> CheckProduct(string id, string idSize);
        DonDat CreateCheckoutUser(CheckOutUserModel model, ProductModel[] items, string id, string IdVNpay, string note);
    }
}
