using FinancialPlanner.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? BudgetItemId { get; set; }
        public string enteredById { get; set; }

        public DateTime date { get; set; }
        [Range(0, 100000000)]
        public int Amount { get; set; }
        public TransactionTypes Type { get; set; }

        public bool Reaconciled { get; set; }
        public decimal ReconciledAmout { get; set; }

        public virtual Account Account { get; set; } 
        public virtual BudgetItem BudgetItem { get; set; }
        public virtual ApplicationUser enteredBy { get; set; }
    }
}