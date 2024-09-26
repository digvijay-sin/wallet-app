using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Expense_Tracker_Core.Models;
using Expense_Tracker.Service;
using Newtonsoft.Json;
using System.Text;


namespace Expense_Tracker.Controllers
{

    public class TransactionController : Controller
    {
        private readonly HttpClient _http;

        public TransactionController(HttpClient http)
        {
            _http = http;
        }        

        [HttpGet("Transaction")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var response = await _http.GetAsync(TransactionURLService.GET_ALL_TRANSACTIONS);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<TransactionRes>? transactions = JsonConvert.DeserializeObject<List<TransactionRes>>(content);
                return Json(new { transactions = transactions });
            }
            return Json(new { transactions = new List<TransactionRes>() });
        }

        public async Task<IActionResult> AddOrEdit(int id)
        {
            await PopulateCategories();
            if (id == 0) { 
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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([FromBody] TransactionReq transaction)
        {
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync(TransactionURLService.DELETE_TRANSACTION + id);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
        public async Task PopulateCategories()
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
