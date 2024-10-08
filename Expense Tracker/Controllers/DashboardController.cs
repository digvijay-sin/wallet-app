﻿using Expense_Tracker.Service;
using Expense_Tracker_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NuGet.Common;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _http;
        private readonly TransactionService _transactService;

        public DashboardController(HttpClient http, TransactionService transactService)
        {
            _http = http;
            _transactService = transactService;
        }

        [HttpGet("Dashboard")]        
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                // Last 7 days transaction
                DateTime StartDate = DateTime.Today.AddDays(-6);
                DateTime EndDate = DateTime.Today;
                List<TransactionRes>? transactions = await _transactService.GetTransactionsAsync(HttpContext.Session.GetString("token"));
                if (transactions == null)
                {
                    ViewBag.TotalIncome = 0.ToString("C0");
                    ViewBag.TotalExpense = 0.ToString("C0");
                    ViewBag.Balance = 0.ToString("C0");
                    return View();
                }

                transactions = transactions.Where(y => y.Date >= StartDate && y.Date <= EndDate).ToList();

                // Total Income
                int TotalIncome = transactions
                    .Where(i => i.Category.Type == "Income")
                    .Sum(j => j.Amount);

                ViewBag.TotalIncome = TotalIncome.ToString("C0");

                // Total Expense
                int TotalExpense = transactions

                    .Where(i => i.Category.Type == "Expense")
                    .Sum(j => j.Amount);
                ViewBag.TotalExpense = TotalExpense.ToString("C0");

                // Balance
                int Balance = TotalIncome - TotalExpense;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-IN");
                culture.NumberFormat.CurrencyNegativePattern = 1;

                ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

                //Doughnut Chart - Expense By Category
                ViewBag.DoughnutChartData = transactions
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
                List<SplineChartData> IncomeSummary = transactions
                    .Where(i => i.Category.Type == "Income")
                    .GroupBy(j => j.Date)
                    .Select(k => new SplineChartData()
                    {
                        day = k.First().Date.ToString("dd-MMM"),
                        income = k.Sum(l => l.Amount)
                    })
                    .ToList();

                //Expense
                List<SplineChartData> ExpenseSummary = transactions
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
                ViewBag.RecentTransactions = transactions
                                    .OrderByDescending(i => i.Date)
                                    .Take(5);
                return View();
            }
            else
            {
                return Redirect("Auth/Login");
            }       
        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;
        }
    }
}
