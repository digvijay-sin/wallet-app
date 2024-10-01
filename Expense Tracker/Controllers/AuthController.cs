using Expense_Tracker.Service;
using Expense_Tracker_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Expense_Tracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;
        public AuthController(HttpClient http)
        {
            _http = http;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.Get("token") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginCred cred)
        {
            var _cred = JsonConvert.SerializeObject(cred);
            var loginContent = new StringContent(_cred, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(AuthURlService.LOGIN, loginContent);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Token after setting: {token}");

                HttpContext.Session.SetString("token", token);
                return Json(new { redirectUrl = @Url.Action("Index", "Dashboard") });
            }

            return Json(new { redirectUrl = @Url.Action("Login", "Auth")});
        }

        [HttpGet("[action]")]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                HttpContext.Session.Clear();                
            }
            return Redirect("Auth/Login");
        }
    }
}

