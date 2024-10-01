using Expense_Tracker_Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Interface
{
    public interface ICategory
    {
        Task<IEnumerable<CategoryRes>> GetCategoriesAsync(int userId);

        Task<CategoryRes> GetCategoryAsync(int id, int userId);

        Task<bool> DeleteCategoryAsync(int id, int userId);

        Task<CategoryRes> UpdateCategoryAsync(int id, CategoryReq category, int userId);

        Task<bool> AddCategoryAsync(CategoryReq category, int userId);

    }
}
