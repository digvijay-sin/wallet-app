using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_API.Controllers
{
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

        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCateogries()
        {
            var categories = await _repository.GetCategoriesAsync();
            return Ok(categories);
        }

        
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category =  await _repository.GetCategoryAsync(id);
            return Ok(category);
        }

        
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryReq newCategory)
        {
            var isSuccess = await _repository.AddCategoryAsync(newCategory);
            return StatusCode(201, "Create Successfully");
        }


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryReq modifiedCategory)
        {
            var updatedCategory = await _repository.UpdateCategoryAsync(id, modifiedCategory);

            return Ok(updatedCategory);
        }

        
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccess = await _repository.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}
