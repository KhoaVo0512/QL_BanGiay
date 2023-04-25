using QL_BanGiay.Areas.Admin.Models;
using QL_BanGiay.Data;

namespace QL_BanGiay.Areas.Admin.Interface
{
    public interface IChart
    {
        double? TotalVans();
        double? TotalConverse();
        List<SplineChartData> IncomeSummary();
        List<SplineChartData> ExpenseSummary();
        double? TotalAdidas();
        double? Total();
        IQueryable<QuantityModel> ListQuantityInCome();
    }
}
