using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }

        [MaxLength(20), MinLength(1)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [Range(0, 100000000)]
        public Decimal TargetTotal { get; set; }
        [Range(0, 100000000)]
        public Decimal CurrentTotal { get; set; }

        public virtual Household Household { get; set; }

        public ICollection<BudgetItem> BudgetItems { get; set; }

        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>(); 
        }

    }
}