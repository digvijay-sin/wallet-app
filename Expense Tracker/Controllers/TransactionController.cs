using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Expense_Tracker_Core.Models;
using Expense_Tracker.Service;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using NuGet.Common;


namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly HttpClient _http;
        private readonly TransactionService _transactService;

        public TransactionController(HttpClient http, TransactionService transactService)
        {
            _http = http;
            _transactService = transactService;
        }        

        [HttpGet("Transaction")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {           
            if (HttpContext.Session.GetString("token") != null)
            {
                var transactions = await _transactService.GetTransactionsAsync(HttpContext.Session.GetString("token"));
                return Json(new { transactions = transactions, status = true });
            }
            return Json(new { transactions = new List<TransactionRes>(), status = false });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                var token = HttpContext.Session.GetString("token");
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await PopulateCategories();
                if (id == 0)
                {
                    return View(new TransactionReq());
                }
                else
                {
                    var transaction = await GetTransaction(id);
                    TransactionReq model = new TransactionReq()
                    {
                        TransactionId = transaction.TransactionId,
                        CategoryId = transaction.CategoryId,
                        Amount = transaction.Amount,
                        Date = transaction.Date,
                        Note = transaction.Note
                    };

                    return View(model);
                }
            }

            return RedirectToAction("Login", "Auth");           
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([FromBody] TransactionReq transaction)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                var token = HttpContext.Session.GetString("token");
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (transaction.TransactionId == 0)
                {
                    var _transaction = JsonConvert.SerializeObject(transaction);
                    var postNewTransaction = new StringContent(_transaction, Encoding.UTF8, "application/json");
                    var response = await _http.PostAsync(TransactionURLService.ADD_TRANSACTION, postNewTransaction);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync();
                        return Json(new { redirectUrl = Url.Action("Index", "Transaction") });
                    }
                    await PopulateCategories();
                    return View(transaction);
                }
                else
                {
                    var _transaction = JsonConvert.SerializeObject(transaction);
                    var putTransaction = new StringContent(_transaction, Encoding.UTF8, "application/json");
                    var response = await _http.PutAsync(TransactionURLService.UPDATE_TRANSACTION + transaction.TransactionId, putTransaction);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync();
                        return Json(new { redirectUrl = Url.Action("Index", "Transaction") });
                    }
                    return View(transaction);
                }
            }
            return Json(new { redirectUrl = Url.Action("Login", "Auth") });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                var token = HttpContext.Session.GetString("token");
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _http.DeleteAsync(TransactionURLService.DELETE_TRANSACTION + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Auth");
        }


        private async Task<TransactionRes?> GetTransaction(int id)
        {
            var response = await _http.GetAsync(TransactionURLService.GET_TRANSACTION + id);
            TransactionRes? transaction = new TransactionRes();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                transaction = JsonConvert.DeserializeObject<TransactionRes>(content);
                return transaction;
            }
            return transaction;
        }

        [NonAction]
        private async Task PopulateCategories()
        {
            CategoryController cat = new CategoryController(_http);
            var result = await cat.GetCategories();
            if (result is JsonResult jsonResult)
            {

                System.Diagnostics.Debug.WriteLine("JsonResult Value: " + JsonConvert.SerializeObject(jsonResult.Value));

                var categoriesProperty = jsonResult.Value.GetType().GetProperty("categories");
                if (categoriesProperty != null)
                {
                    CategoryRes defaultCategory = new CategoryRes() {CategoryId = 0, Title = "Choose a Category" };
                    
                    var categories = categoriesProperty.GetValue(jsonResult.Value) as List<CategoryRes>;
                    categories.Insert(0, defaultCategory);
                    ViewBag.Categories = categories ?? new List<CategoryRes>();
                }
            }
            else
            {
                ViewBag.Categories = new List<CategoryRes>();
            }
        }
    }
}
