using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Core.Models
{
    public class TransactionReq
    {
        
        public int TransactionId { get; set; } = 0;

        [Range(1, int.MaxValue, ErrorMessage = "Please Select a Category.")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be Greater than 0.")]
        public int Amount { get; set; }
        
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
