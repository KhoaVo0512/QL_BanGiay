using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using QL_BanGiay.Areas.Admin.Interface;
using QL_BanGiay.Areas.Admin.Models;
using System.Globalization;

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
        private readonly IToastNotification _toastNotification;
        public HomeController(IOrder order, IUser user, IPurchaseOrder purchaseOrder, IChart chartRepo, IToastNotification toastNotification)
        {
            _OrderRepo = order;
            _UserRepo = user;
            _PurchaseOrderRepo = purchaseOrder;
            _ChartRepo = chartRepo;
            _toastNotification = toastNotification;
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
                new DoughnutChartData { Browser= "Adidas", Total= totalAdidas, DataLabelMappingName= "Adidas: " + String.Format(elGR, "{0:0,0}", totalAdidas) +" VND" },
                new DoughnutChartData { Browser= "Converse", Total=totalConverse, DataLabelMappingName= "Converse: "+ String.Format(elGR, "{0:0,0}", totalConverse)+" VND" },
                new DoughnutChartData { Browser= "Nike", Total= totalNike, DataLabelMappingName= "Nike: "+ String.Format(elGR, "{0:0,0}", totalNike)+" VND" },
                new DoughnutChartData { Browser= "Vans", Total= totalVans, DataLabelMappingName= "Vans:" + String.Format(elGR, "{0:0,0}", totalVans) +" VND"},
            };
            ChartPoints.OrderByDescending(x => x.Total).ToList();
            ViewBag.ChartPoints = ChartPoints;
            //LineChart 
            DateTime StartDate = DateTime.Today.AddDays(-6);
            var Income = _ChartRepo.IncomeSummary();
            var Expense = _ChartRepo.ExpenseSummary();
            string[] Last7Days = Enumerable.Range(0, 7)
              .Select(i => StartDate.AddDays(i).ToString("dd-MM"))
              .ToArray();
            ViewBag.SplineChartData = from day in Last7Days
                                      join income in Income on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in Expense on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new SplineChartData
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            return View();
        }
        [Authorize]
        [Route("admin/sigout")]
        public async Task<IActionResult> SigOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            _toastNotification.AddSuccessToastMessage("Đăng xuất thành công");
            return Redirect("/");
        }
        private class DoughnutChartData
        {
            public string Browser;
            public double? Total;
            public string DataLabelMappingName;
        }

    }

}
