using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IDistrict
    {
        Task<List<Huyen>> GetDistricts(string Id);

        List<Huyen> GetDistrict(string id);
        List<Huyen> GetDistrict_MaTinh(string id);
    }
}
