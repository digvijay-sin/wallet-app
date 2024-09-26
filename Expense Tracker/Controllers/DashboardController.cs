using Expense_Tracker_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Newtonsoft.Json;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _http;

        public DashboardController(HttpClient http)
        {
            _http = http;
        }

        [HttpGet("Dashboard")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {

            // Last 7 days transaction
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            TransactionController transact = new TransactionController(_http);

            var transactionsAnonymous = await transact.GetTransactions();

            List<TransactionRes> SelectedTransactions = new List<TransactionRes>();
            var transactions = new List<TransactionRes>();
            if (transactionsAnonymous is JsonResult jsonResult)
            {
                var transactionProperty = jsonResult.Value.GetType().GetProperty("transactions");

                if (transactionProperty != null)
                {
                    transactions = transactionProperty.GetValue(jsonResult.Value) as List<TransactionRes>;
                    // Filter based on date
                    SelectedTransactions = transactions.Where(y => y.Date >= StartDate && y.Date <= EndDate).ToList();
                }
                // Total Income
                int TotalIncome = SelectedTransactions
                    .Where(i => i.Category.Type == "Income")
                    .Sum(j => j.Amount);

                ViewBag.TotalIncome = TotalIncome.ToString("C0");

                // Total Expense
                int TotalExpense = SelectedTransactions

                    .Where(i => i.Category.Type == "Expense")
                    .Sum(j => j.Amount);
                ViewBag.TotalExpense = TotalExpense.ToString("C0");

                // Balance
                int Balance = TotalIncome - TotalExpense;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-IN");
                culture.NumberFormat.CurrencyNegativePattern = 1;

                ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

                //Doughnut Chart - Expense By Category
                ViewBag.DoughnutChartData = SelectedTransactions
                    .Where(t => t.Category.Type == "Expense")
                    .GroupBy(c => c.Category.CategoryId)
                    .Select(k => new
                    {
                        categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                        amount = k.Sum(j => j.Amount),
                        formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                    })
                    .OrderByDescending(l => l.amount)
                    .ToList();

                //Spline Chart - Income Vs Expense
                // Income
                List<SplineChartData> IncomeSummary = SelectedTransactions
                    .Where(i => i.Category.Type == "Income")
                    .GroupBy(j => j.Date)
                    .Select(k => new SplineChartData()
                    {
                        day = k.First().Date.ToString("dd-MMM"),
                        income = k.Sum(l => l.Amount)
                    })
                    .ToList();

                //Expense
                List<SplineChartData> ExpenseSummary = SelectedTransactions
                    .Where(i => i.Category.Type == "Expense")
                    .GroupBy(j => j.Date)
                    .Select(k => new SplineChartData()
                    {
                        day = k.First().Date.ToString("dd-MMM"),
                        expense = k.Sum(l => l.Amount)
                    })
                    .ToList();

                // Combine Income & Expense by their Date
                string[] Last7Days = Enumerable.Range(0, 7)
                    .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                    .ToArray();

                ViewBag.SplineChartData = from day in Last7Days
                                          join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                          from income in dayIncomeJoined.DefaultIfEmpty()
                                          join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                          from expense in expenseJoined.DefaultIfEmpty()
                                          select new
                                          {
                                              day = day,
                                              income = income == null ? 0 : income.income,
                                              expense = expense == null ? 0 : expense.expense

                                          };


                //Recent Transactions
                ViewBag.RecentTransactions =  transactions
                                    .OrderByDescending(i => i.Date)
                                    .Take(5);
            }
            else
            {
                ViewBag.TotalIncome = 0.ToString("C0");
                ViewBag.TotalExpense = 0.ToString("C0");
                ViewBag.Balance = 0.ToString("C0");                
            }
            return View();
        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;
        }
    }
}
