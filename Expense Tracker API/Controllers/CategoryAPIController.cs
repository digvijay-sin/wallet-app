using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Tracker_API.Controllers
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    
    public class CategoryAPIController : ControllerBase
    {
        private readonly ICategory _repository;

        public CategoryAPIController(ICategory repository)
        {
            _repository = repository;
        }

        [NonAction]
        private int UserId() {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (userId == null)
            {
                return 0;
            }
            return int.Parse(userId);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCateogries()
        {
            var categories = await _repository.GetCategoriesAsync(UserId());
            return Ok(categories);
        }

        
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category =  await _repository.GetCategoryAsync(id, UserId());
            return Ok(category);
        }

        
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryReq newCategory)
        {
            var isSuccess = await _repository.AddCategoryAsync(newCategory, UserId());
            return StatusCode(201, "Create Successfully");
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryReq modifiedCategory)
        {
            var updatedCategory = await _repository.UpdateCategoryAsync(id, modifiedCategory, UserId());

            return Ok(updatedCategory);
        }

        
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _repository.DeleteCategoryAsync(id, UserId());

            return NoContent();
        }
    }
}
