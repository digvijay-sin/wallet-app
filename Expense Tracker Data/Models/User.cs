using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Column("Phone", TypeName = "nvarchar(20)")]
        public string Phone { get; set; }
        [Column("Email", TypeName = "nvarchar(50)")]
        public string Email { get; set; }
        [Column("Password", TypeName = "nvarchar(100)")]
        public int Password{ get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
