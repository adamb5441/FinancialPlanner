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
        public void updateBudget(int budgetId)
        {
            var budget = db.Budgets.Find(budgetId);
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            
            var items = db.BudgetItems.Where(i => i .date>startDate && i.BudgetId == budgetId);
            budget.CurrentTotal = items.Sum(i => i.Cost);
            db.SaveChanges();
        }
        public bool isOverBudget(int budgetId)
        {
            var budget = db.Budgets.Find(budgetId);
            if(budget.CurrentTotal > budget.TargetTotal)
            {
                return true;
            }
            return false;
        }
    }
}