using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface ICommune
    {
        Task<List<Xa>> GetCommunes(string Id);
    }
}
