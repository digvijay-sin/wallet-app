using Expense_Tracker_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Expense_Tracker.Service
{
    public class TransactionService : Controller
    {
        private readonly HttpClient _http;

        public TransactionService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TransactionRes>?> GetTransactionsAsync(string? token)
        {
            List<TransactionRes>? transactions = new List<TransactionRes>();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync(TransactionURLService.GET_ALL_TRANSACTIONS);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                transactions = JsonConvert.DeserializeObject<List<TransactionRes>>(content);
                return transactions;
            }
            return transactions;
        }
    }
}
