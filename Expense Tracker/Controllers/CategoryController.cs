using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


using Expense_Tracker_Core.Models;
using Expense_Tracker.Service;
using Newtonsoft.Json;
using System.Text;
using Humanizer;

namespace Expense_Tracker.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly HttpClient _http;

        public CategoryController(HttpClient http)
        {
            _http = http;
        }


        [HttpGet("Category")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _http.GetAsync(CategoryURLService.GET_ALL_CATEGORIES);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<CategoryRes>? categories = JsonConvert.DeserializeObject<List<CategoryRes>>(content);
                return Json(new { categories = categories });
            }
            return Json(new { categories = new List<CategoryRes>() });
        }


        //public async Task<IActionResult> Details(int id)
        //{
        //    var category = await GetCategory(id);
        //    return View(category);
        //}

        [HttpGet, ActionName("AddOrEdit")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                CategoryReq defaultCategory = new CategoryReq();
                return View(defaultCategory);
            }
            else
            {
                var category = await GetCategory(id);
                CategoryReq _category = new CategoryReq
                {
                    CategoryId = category.CategoryId,
                    Icon = category.Icon,
                    Title = category.Title,
                    Type = category.Type
                };
                return View(_category);
            }

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([FromBody] CategoryReq category)
        {
            if (category.CategoryId == 0)
            {
                var _category = JsonConvert.SerializeObject(category);
                var postNewCategory = new StringContent(_category, Encoding.UTF8, "application/json");
                var response = await _http.PostAsync(CategoryURLService.ADD_CATEGORY, postNewCategory);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync();
                    return Json(new { redirectUrl = Url.Action("Index", "Category")});
                }
                return View(category);
            }
            else
            {
                var _category = JsonConvert.SerializeObject(category);
                var putCategory = new StringContent(_category, Encoding.UTF8, "application/json");
                var response = await _http.PutAsync(CategoryURLService.UPDATE_CATEGORY + category.CategoryId, putCategory);
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync();
                    return Json(new { redirectUrl = Url.Action("Index", "Category") });
                }
                return View(category);
            }

        }


        //public async Task<IActionResult> Edit(int id )
        //{            
        //    var category = await GetCategory(id);
        //    return View(category);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Title,Icon,Type")] CategoryReq category)
        //{

        //    var _category = JsonConvert.SerializeObject(category);
        //    var putCategory = new StringContent(_category, Encoding.UTF8, "application/json");
        //    var response = await _http.PutAsync(URLService.UPDATE_CATEGORY + id, putCategory);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = response.Content.ReadAsStringAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}


        //public async Task<IActionResult> Delete(int id)
        //{
        //    var category = await GetCategory(id);
        //    return View(category);
        //}


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync(CategoryURLService.DELETE_CATEGORY + id);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return View();
        }


        private async Task<CategoryRes?> GetCategory(int id)
        {

            var response = await _http.GetAsync(CategoryURLService.GET_CATEGORY + id);
            CategoryRes? category = new CategoryRes();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                category = JsonConvert.DeserializeObject<CategoryRes>(content);
                return category;
            }
            return category;
        }
    }
}
