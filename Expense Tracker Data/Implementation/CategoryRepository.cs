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

        public async Task<bool> AddCategoryAsync(CategoryReq category)
        {
            var newCategory = _mapper.Map<Category>(category);
            await _context.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) {
                return false;
            }
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryRes>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            List<CategoryRes> allCategories = new List<CategoryRes>();

            foreach (var category in categories) { 
                var c = _mapper.Map<CategoryRes>(category);
                allCategories.Add(c);
            }

            return allCategories;            
        }

        public async Task<CategoryRes> GetCategoryAsync(int id)
        {
            var _category = await _context.Categories.FindAsync(id);
            var category = _mapper.Map<CategoryRes>(_category);
            return category;
        }

        public async Task<CategoryRes> UpdateCategoryAsync(int id, CategoryReq modifiedCategory)
        {
            var _category = await _context.Categories.FindAsync(id);

            _context.Entry(_category).CurrentValues.SetValues(modifiedCategory);
            _context.Entry(_category).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var category = _mapper.Map<CategoryRes>(_category);

            return category;
        }
    }
}
