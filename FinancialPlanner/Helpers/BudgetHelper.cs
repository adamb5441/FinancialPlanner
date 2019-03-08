using FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Helpers
{
    public class BudgetHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public decimal GetCurrentBudget(int Id)
        {
            var budget = db.Budgets.Find(Id);

            var expenses = budget.BudgetItems.Sum(b => b.Cost);

            return expenses;
        }


    }
}