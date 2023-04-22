using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QL_BanGiay.Areas.Admin.Interface;
using System.Globalization;
using Windows.UI.Xaml.Controls;

namespace QL_BanGiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Emloyee")]
    //[Authorize(Policy = "Admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly IOrder _OrderRepo;
        private readonly IUser _UserRepo;
        private readonly IPurchaseOrder _PurchaseOrderRepo;
        private readonly IChart _ChartRepo;
        public HomeController(IOrder order, IUser user, IPurchaseOrder purchaseOrder, IChart chartRepo)
        {
            _OrderRepo = order;
            _UserRepo = user;
            _PurchaseOrderRepo = purchaseOrder;
            _ChartRepo = chartRepo;
        }
        public IActionResult Index()
        {
            ViewBag.CountDonDat = _OrderRepo.GetCountDonDat();
            ViewBag.CountUser = _UserRepo.GetUserCount();
            CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");
            ViewBag.TotalIncome = String.Format(elGR, "{0:0,0}", _OrderRepo.GetTotalInCome());
            ViewBag.Exprense = String.Format(elGR, "{0:0,0}", _PurchaseOrderRepo.GetTotalExprense());
            var Balance = _OrderRepo.GetTotalInCome() - _PurchaseOrderRepo.GetTotalExprense();
            ViewBag.Balance = String.Format(elGR, "{0:0,0}", Balance);

            var totalConverse = _ChartRepo.TotalConverse();
            var totalAdidas = _ChartRepo.TotalAdidas();
            var totalVans = _ChartRepo.TotalVans();

            var totalNike = _OrderRepo.GetTotalInCome() - (totalConverse + totalAdidas + totalVans);
            List<DoughnutChartData> ChartPoints = new List<DoughnutChartData>
            {
                new DoughnutChartData { Browser= "Adidas", Total= totalAdidas, DataLabelMappingName= "Adidas: " + totalAdidas +" VND" },
                new DoughnutChartData { Browser= "Converse", Total=totalConverse, DataLabelMappingName= "Converse: "+ totalConverse+" VND" },
                new DoughnutChartData { Browser= "Nike", Total= totalNike, DataLabelMappingName= "Nike: "+ totalNike+" VND" },
                new DoughnutChartData { Browser= "vans", Total= totalVans, DataLabelMappingName= "Vans:" + totalVans +" VND"},
            };
            ViewBag.ChartPoints = ChartPoints;

            return View();
        }
        private class DoughnutChartData
        {
            public string Browser;
            public double? Total;
            public string DataLabelMappingName;
        }
    }

}
