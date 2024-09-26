using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Core.Models
{
    public class CategoryRes
    {

        public int CategoryId { get; set; }
        public string Title { get; set; }        
        public string Icon { get; set; }         
        public string Type { get; set; }

        [NotMapped]
        public string? TitleWithIcon{ 
            get 
            {
                return this.Icon + " " + this.Title;
            }
        }

    }
}
