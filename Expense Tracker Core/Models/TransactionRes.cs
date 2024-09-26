using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Core.Models
{
    public class TransactionRes
    {
        
        public int TransactionId { get; set; } = 0;

        public int CategoryId { get; set; }
        
        public CategoryRes Category { get; set; }
        
        public int Amount { get; set; }
        
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon{
            get {
                return Category == null ? "" : Category.Icon + " " + Category.Title;
            }
        }

        [NotMapped]
        public string? FomattedAmount { 
            get {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
    }
}
