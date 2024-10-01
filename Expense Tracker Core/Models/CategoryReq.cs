using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Core.Models
{
    public class CategoryReq
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        
        public string? Icon { get; set; }

        public string Type { get; set; } = "Expense";
    }
}
