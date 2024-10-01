using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }       

        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note{ get; set; }

        public DateTime Date { get; set; } =  DateTime.Now;

        public int CategoryId { get; set; }

        public int UserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("UserId")]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public User User { get; set; }

    }
}
