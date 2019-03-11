using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        [Required, MaxLength(40), MinLength(1)]
        public string Name { get; set; }
        [Required, MaxLength(40), MinLength(1)]
        public string Description { get; set; }
        [Range(0, 100000000)]
        public decimal Cost { get; set; }
        public DateTime date { get; set; }

        public virtual Budget Budget { get; set; }
    }
}