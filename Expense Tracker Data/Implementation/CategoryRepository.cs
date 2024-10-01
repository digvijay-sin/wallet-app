using AutoMapper;
using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Context;
using Expense_Tracker_Data.Interface;
using Expense_Tracker_Data.Models;
using Microsoft.EntityFrameworkCore;



namespace Expense_Tracker_Data.Implementation
{
    public class CategoryRepository : ICategory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddCategoryAsync(CategoryReq category, int userId)
        {
            var newCategory = _mapper.Map<Category>(category);
            newCategory.UserId = userId;
            await _context.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id, int userId)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            if (category.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryRes>> GetCategoriesAsync(int userId)
        {
            var categories = await _context.Categories.Where(c => c.UserId == userId).ToListAsync();

            List<CategoryRes> allCategories = new List<CategoryRes>();

            foreach (var category in categories)
            {
                var c = _mapper.Map<CategoryRes>(category);
                allCategories.Add(c);
            }

            return allCategories;
        }

        public async Task<CategoryRes?> GetCategoryAsync(int id, int userId)
        {
            var _category = await _context.Categories.FindAsync(id);
            if(_category == null)
            {
                return null;
            }
            if (_category.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }
            var category = _mapper.Map<CategoryRes>(_category);
            return category;
        }

        public async Task<CategoryRes> UpdateCategoryAsync(int id, CategoryReq modifiedCategory, int userId)
        {
            var _category = await _context.Categories.FindAsync(id);

            if(_category == null)
            {
                return null;
            }

            if (_category.UserId != userId)
            {
                throw new Exception("Unauthorized");
            }

            _context.Entry(_category).CurrentValues.SetValues(modifiedCategory);
            _context.Entry(_category).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var category = _mapper.Map<CategoryRes>(_category);

            return category;
        }
    }
}
