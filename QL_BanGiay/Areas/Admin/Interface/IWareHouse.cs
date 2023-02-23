using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IWareHouse
    {
        bool IsShoeSizeNoExists(string? maGiay, int? maSize);
        KhoGiay GetItemWareHouse(string? maGiay, int? maSize);
    }
}
