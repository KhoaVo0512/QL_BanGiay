using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IDistrict
    {
        Task<List<Huyen>> GetDistricts(string Id);
    }
}
