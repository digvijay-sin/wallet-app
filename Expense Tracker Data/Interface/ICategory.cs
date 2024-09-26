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
        Task<IEnumerable<CategoryRes>> GetCategoriesAsync();

        Task<CategoryRes> GetCategoryAsync(int id);

        Task<bool> DeleteCategoryAsync(int id);

        Task<CategoryRes> UpdateCategoryAsync(int id, CategoryReq category);

        Task<bool> AddCategoryAsync(CategoryReq category);

    }
}
