using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IChart
    {
        double? TotalVans();
        double? TotalConverse();
        double? TotalAdidas();
        double? Total();
    }
}
