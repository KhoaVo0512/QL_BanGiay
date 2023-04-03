using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IProvince
    {
        Task<List<Tinh>> GetProvinces();
        List<Tinh> GetProvince();
        List<Tinh> GetProvince(string id);
    }
}
